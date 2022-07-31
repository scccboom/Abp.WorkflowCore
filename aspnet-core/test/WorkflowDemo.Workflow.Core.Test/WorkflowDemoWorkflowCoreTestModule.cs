using Abp.Modules;

using WorkflowDemo.Workflows;

namespace WorkflowDemo.Workflow.Test
{
    [DependsOn(typeof(WorkflowDemoWorkflowCoreModule))]
    public class WorkflowDemoWorkflowCoreTestModule : AbpModule
    {
    }
}