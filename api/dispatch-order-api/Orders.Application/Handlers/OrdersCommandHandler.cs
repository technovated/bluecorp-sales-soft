using MediatR;
using Orders.Application.Commands;
using Orders.Core;
using Orders.Core.Entities;
using Orders.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Handlers
{
    public class OrdersCommandHandler : IRequestHandler<OrderDispatchCommand, bool>
    {
        private readonly ISftpRepository _sftpRepository;
        public OrdersCommandHandler(ISftpRepository sftpRepository)
        {
            _sftpRepository = sftpRepository;
        }

        public async Task<bool> Handle(OrderDispatchCommand request, CancellationToken cancellationToken)
        {
            string csvContent = JsonContentUtility.ConvertJSONToCsv(request.Payload);
            return _sftpRepository.Dispatch(csvContent, request.Payload.SalesOrder);
        }


    }
}
