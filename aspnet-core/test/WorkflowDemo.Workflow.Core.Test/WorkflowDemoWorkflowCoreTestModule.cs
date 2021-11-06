using Abp.Modules;

namespace WorkflowDemo.Workflow.Test
{
    [DependsOn(typeof(WorkflowDemoWorkflowCoreModule))]
    public class WorkflowDemoWorkflowCoreTestModule : AbpModule
    {
    }
}