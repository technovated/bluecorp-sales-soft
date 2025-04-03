using MediatR;
using Orders.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Commands
{
    public class OrderDispatchCommand : IRequest<bool>
    {
        private OrderPayload _orderPayload;
        public OrderDispatchCommand(OrderPayload orderPayload)
        {
            _orderPayload = orderPayload;
        }

        public OrderPayload Payload
        {
            get
            {
                return _orderPayload;
            }
        }
    }
}
