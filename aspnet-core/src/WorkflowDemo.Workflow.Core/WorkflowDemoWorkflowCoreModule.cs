using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero;

namespace WorkflowDemo.Workflows
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(AbpZeroCoreModule))]
    public class WorkflowDemoWorkflowCoreModule : AbpModule
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            IocManager.IocContainer.Install(new WorkflowInstaller());

            var thisAssembly = typeof(WorkflowDemoWorkflowCoreModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly, new ConventionalRegistrationConfig
            {
                InstallInstallers = false
            });
        }
    }
}