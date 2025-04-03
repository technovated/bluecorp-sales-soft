using MediatR;
using Orders.Application.Commands;
using Orders.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Handlers
{
    public class FailedDispatchHandler : IRequestHandler<FailedDispatchCommand, bool>
    {
        private readonly IFailedDispatchStoreRespository _failedDispatchStoreRespository;
        public FailedDispatchHandler(IFailedDispatchStoreRespository failedDispatchStoreRespository)
        {
            _failedDispatchStoreRespository = failedDispatchStoreRespository;
        }
        public Task<bool> Handle(FailedDispatchCommand request, CancellationToken cancellationToken)
        {
            return _failedDispatchStoreRespository.Persist(request.Payload);
        }
    }
}
