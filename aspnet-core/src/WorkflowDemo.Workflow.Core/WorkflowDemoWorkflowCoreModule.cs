using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero;

namespace WorkflowDemo.Workflow
{
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class WorkflowDemoWorkflowCoreModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WorkflowDemoWorkflowCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<WorkflowDefinitionManager>().Initialize();
        }
    }
}