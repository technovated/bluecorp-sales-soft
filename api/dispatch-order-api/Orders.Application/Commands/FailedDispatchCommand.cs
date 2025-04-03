using MediatR;
using Orders.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Commands
{
    public class FailedDispatchCommand : OrderDispatchCommand
    {
        public FailedDispatchCommand(OrderPayload orderPayload) : base(orderPayload)
        {
        }
    }
}
