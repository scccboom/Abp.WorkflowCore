using Abp.AutoMapper;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;

using WorkflowDemo.Authorization;
using WorkflowDemo.Workflows;

namespace WorkflowDemo
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(
        typeof(WorkflowDemoCoreModule),
        typeof(WorkflowDemoWorkflowCoreModule),
        typeof(AbpAutoMapperModule))]
    public class WorkflowDemoApplicationModule : AbpModule
    {
        /// <summary>
        /// 
        /// </summary>
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<WorkflowDemoAuthorizationProvider>();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            var thisAssembly = typeof(WorkflowDemoApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}