using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Core
{
    public class SftpUtility<T>
    {
        private readonly ILogger<T> _logger;
        public SftpUtility(ILogger<T> logger)
        {
            _logger = logger;
        }
        public bool SendCSV(string csvPayload, string orderHead)
        {
            bool success = false;
            int attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("RETRY_COUNT") ?? "2") + 1;

            do
            {
                success = TrySend(csvPayload, orderHead);
                attempts--;
            } while (success == false && attempts > 0);
            return success;
        }

        private bool TrySend(string payload, string orderHeader)
        {
            bool success = false;

            string sftpHost = Environment.GetEnvironmentVariable("SFTP_HOST");
            string sftpUser = Environment.GetEnvironmentVariable("SFTP_USER");
            string sftpPassword = Environment.GetEnvironmentVariable("SFTP_PASSWORD");
            string sftpFolder = Environment.GetEnvironmentVariable("SFTP_FOLDER") ?? "/upload";
            string connectionString = Environment.GetEnvironmentVariable("AzureStorage");
            try
            {
                var _blobServiceClient = new BlobServiceClient(connectionString);
                var containerClient = _blobServiceClient.GetBlobContainerClient("bluecorp-container");
                var blobClient = containerClient.GetBlobClient($"dispatch_{orderHeader}.csv");

                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(payload));
                var response = blobClient.Upload(stream, overwrite: true);

                success = (response.GetRawResponse() as Azure.Response).Status == (int)HttpStatusCode.Created;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SFTP Upload Error: {ex.Message}");
                success = false;
            }

            return success;
        }
    }
}
