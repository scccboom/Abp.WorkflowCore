using System.Collections.Generic;
using System.Threading.Tasks;

namespace WorkflowDemo.Workflow
{
    public interface IAbpWorkflowManager
    {
        Task<bool> TerminateWorkflow(string workflowId);

        IEnumerable<AbpWorkflowStepBody> GetAllStepBodys();

        Task PublishEventAsync(string eventName, string eventKey, object eventData);

        Task CreateAsync(PersistedWorkflowDefinition entity);

        Task DeleteAsync(string id);

        Task UpdateAsync(PersistedWorkflowDefinition entity);

        Task StartWorlflow(string id, int version, Dictionary<string, object> inputs);
    }
}