using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Orders.Core;
using Orders.Core.Repositories;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Net;
using System.Text;

namespace Orders.Infrastructure
{
    public class SftpRepository : ISftpRepository
    {
        private readonly ILogger<SftpRepository> _logger;
        public SftpRepository(ILogger<SftpRepository> logger)
        {
            _logger = logger;
        }

        public bool Dispatch(string csvPayload, string orderHead)
        {
            SftpUtility<ISftpRepository> sftpUtility = new SftpUtility<ISftpRepository>(_logger);
            return sftpUtility.SendCSV(csvPayload, orderHead);
        }
    }
}
