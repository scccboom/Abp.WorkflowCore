using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Abp.Domain.Repositories;

namespace WorkflowDemo.Workflows
{
    public interface IAbpWorkflowRepository : IRepository<PersistedWorkflow, string>
    {
        Task<PersistedWorkflow> GetByIdWithDetailsAsync(string id);

        Task<PersistedWorkflow> GetByUserIdAsync(long? userId);

        IQueryable<PersistedWorkflow> GetAllThenDetail(bool isTracking = false);
    }
}
