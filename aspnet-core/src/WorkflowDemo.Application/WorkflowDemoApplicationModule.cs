using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

using WorkflowCore.Interface;

using WorkflowDemo.Application.Workflows.StepBodys;
using WorkflowDemo.Authorization;
using WorkflowDemo.Workflow;

namespace WorkflowDemo
{
    [DependsOn(
        typeof(WorkflowDemoCoreModule),
        typeof(WorkflowDemoWorkflowCoreModule),
        typeof(AbpAutoMapperModule))]
    public class WorkflowDemoApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<WorkflowDemoAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(WorkflowDemoApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );

            Configuration.GetWorkflowConfiguration().Providers.Add<DefaultStepBodyProvider>();

            IocManager.IocContainer.Install(new WorkflowInstaller(IocManager));
        }

        public override void PostInitialize()
        {
            var host = IocManager.Resolve<IWorkflowHost>();
            host.Start();
            IocManager.Resolve<AbpWorkflowManager>().Initialize();
        }

        public override void Shutdown()
        {
            IocManager.Resolve<IWorkflowHost>().Stop();
        }
    }
}