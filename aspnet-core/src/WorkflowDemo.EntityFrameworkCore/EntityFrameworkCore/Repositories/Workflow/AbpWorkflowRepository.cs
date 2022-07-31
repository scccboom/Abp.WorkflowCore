using System;
using System.Linq;
using System.Threading.Tasks;

using Abp.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

using WorkflowDemo.EntityFrameworkCore;
using WorkflowDemo.EntityFrameworkCore.Repositories;

namespace WorkflowDemo.Workflows
{
    public class AbpWorkflowRepository : WorkflowDemoRepositoryBase<PersistedWorkflow, string>, IAbpWorkflowRepository
    {
        public AbpWorkflowRepository(IDbContextProvider<WorkflowDemoDbContext> dbContextProvider) 
            : base(dbContextProvider)
        {
        }

        public Task<PersistedWorkflow> GetByIdWithDetailsAsync(string id)
        {
            return GetAllIncluding(x => x.ExecutionPointers, x => x.WorkflowDefinition)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<PersistedWorkflow> GetByUserIdAsync(long? userId)
        {
            return FirstOrDefaultAsync(x => x.CreatorUserId == userId);
        }

        public IQueryable<PersistedWorkflow> GetAllThenDetail(bool isTracking = false)
        {
            IQueryable<PersistedWorkflow> query = GetAll()
                .Include(x => x.ExecutionPointers)
                .ThenInclude(x => x.ExtensionAttributes);

            if (isTracking)
            {
                query = query.AsTracking();
            }

            return query;
        }
    }
}
