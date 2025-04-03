using Orders.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Core.Repositories
{
    public interface IFailedDispatchStoreRespository
    {
        Task QueueInit();
        Task<bool> Persist(OrderPayload payload);
    }
}
