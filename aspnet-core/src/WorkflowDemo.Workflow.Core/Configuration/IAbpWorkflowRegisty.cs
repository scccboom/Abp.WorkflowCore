using System;

namespace WorkflowDemo.Workflows
{
    public interface IAbpWorkflowRegistry
    {
        void RegisterWorkflow(Type type);
    }
}