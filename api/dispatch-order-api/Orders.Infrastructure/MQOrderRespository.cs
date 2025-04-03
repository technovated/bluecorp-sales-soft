using Microsoft.Extensions.Logging;
using Orders.Core.Entities;
using Orders.Core.Repositories;
using RabbitMQ.Client;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Orders.Infrastructure
{
    public class MQOrderRespository : IFailedDispatchStoreRespository
    {
        private readonly ILogger<MQOrderRespository> _logger;
        private readonly IConnection _connection;
        private IChannel _channel;
        string exchangeName = "orders-exchange";
        string queueName = "failed_orders_queue";
        string routingKey = "blue_corp_orders";
        public MQOrderRespository(ILogger<MQOrderRespository> logger, IConnection connection)
        {
            _logger = logger;
            _connection = connection;
        }
        public async Task QueueInit()
        {
            await using var channel = await _connection.CreateChannelAsync();
            _channel = channel;


            await _channel.ExchangeDeclareAsync(exchangeName, ExchangeType.Direct, durable: true);
            await _channel.QueueDeclareAsync(queueName, durable: true, exclusive: false, autoDelete: false);
            await _channel.QueueBindAsync(queueName, exchangeName, routingKey);
        }
        public async Task<bool> Persist(OrderPayload payload)
        {
            bool result = false;
            try
            {
                string message = JsonSerializer.Serialize(payload);

                var properties = new BasicProperties
                {
                    ContentType = "text/plain",
                    DeliveryMode = DeliveryModes.Persistent // Persistent message
                };

                if (_channel == null)
                {
                    var channel = await _connection.CreateChannelAsync();
                    _channel = channel;
                }
                var body = Encoding.UTF8.GetBytes(message);
                await _channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                mandatory: false,
                basicProperties: properties,
                body: body
                 );
                result = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Error while publishing to queue: {0}", ex.Message));
                _logger.LogError(ex.StackTrace);
                result = false;
            }
            return result;
        }


    }
}
