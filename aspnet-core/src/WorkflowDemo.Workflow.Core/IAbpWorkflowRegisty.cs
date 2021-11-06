using System;

namespace WorkflowDemo.Workflow
{
    public interface IAbpWorkflowRegistry
    {
        void RegisterWorkflow(Type type);
    }
}