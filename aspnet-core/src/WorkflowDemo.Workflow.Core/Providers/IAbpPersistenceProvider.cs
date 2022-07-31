using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using WorkflowCore.Interface;

namespace WorkflowDemo.Workflows
{
    public interface IAbpPersistenceProvider : IPersistenceProvider
    {
        Task<PersistedWorkflow> GetPersistedWorkflow(string id);

        Task<IEnumerable<PersistedWorkflow>> GetAllRunnablePersistedWorkflow(string definitionId, int version);

        Task<PersistedExecutionPointer> GetPersistedExecutionPointer(string id);

        Task<PersistedWorkflowDefinition> GetPersistedWorkflowDefinition(string id, int version);
    }
}