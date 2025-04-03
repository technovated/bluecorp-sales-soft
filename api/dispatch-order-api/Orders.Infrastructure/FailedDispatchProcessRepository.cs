using Microsoft.Extensions.Logging;
using Orders.Core;
using Orders.Core.Entities;
using Orders.Core.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using System.Threading.Tasks;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace Orders.Infrastructure
{
    public class FailedDispatchProcessRepository : IFailedDispatchProcessRepository
    {
        private readonly ILogger<FailedDispatchProcessRepository> _logger;
        private readonly IConnection _connection;
        private readonly IChannel _channel;
        public FailedDispatchProcessRepository(ILogger<FailedDispatchProcessRepository> logger, IConnection connection)
        {
            _logger = logger;
            _connection = connection;

            _channel = _connection.CreateChannelAsync().Result; // Blocking for simplicity, avoid in async methods
            InitializeQueue();
        }

        private void InitializeQueue()
        {
            /*_channel.QueueDeclareAsync(
             queue: "failed_orders_queue",
             durable: false,
             exclusive: false,
             autoDelete: false,
             arguments: null).GetAwaiter(); */
        }

        public async Task<bool> Redispatch()
        {
            bool success = false;
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (model, ea) =>
             {
                 var body = ea.Body.ToArray();
                 string message = Encoding.UTF8.GetString(body);

                 if (!string.IsNullOrEmpty(message))
                 {
                     if (ProcessMessage(message))
                     {
                         await Task.Delay(1000);
                         await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);                         
                     }
                     else
                     {
                         await Task.Delay(1000);
                         await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                     }
                 }
                 
             };

            // Start consuming messages from the queue
            string message = await _channel.BasicConsumeAsync(
                  queue: "failed_orders_queue",
                  autoAck: false,
                  consumer: consumer);
            success = true;
            return success;

        }

        private bool ProcessMessage(string message)
        {
            bool processed = false;
            try
            {
                OrderPayload? payload = JsonSerializer.Deserialize<OrderPayload>(message);
                if (payload != null)
                {
                    string csvContent = JsonContentUtility.ConvertJSONToCsv(payload);
                    SftpUtility<IFailedDispatchProcessRepository> sftpUtility = new SftpUtility<IFailedDispatchProcessRepository>(_logger);
                    processed = sftpUtility.SendCSV(csvContent, payload.SalesOrder);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unable to process incoming queue message");
                _logger.LogError(ex.StackTrace);
            }
            return processed;
        }

        public async Task Close()
        {
            await _channel.CloseAsync();
            await _connection.CloseAsync();
        }
    }
}
