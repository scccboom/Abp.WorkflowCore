using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;

using Microsoft.AspNetCore.Mvc.ApplicationParts;

using WorkflowDemo.EntityFrameworkCore;

namespace WorkflowDemo.Web.Tests
{
    [DependsOn(
        typeof(WorkflowDemoWebCoreModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class WorkflowDemoWebTestModule : AbpModule
    {
        public WorkflowDemoWebTestModule(WorkflowDemoEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        }

        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WorkflowDemoWebTestModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(WorkflowDemoWebCoreModule).Assembly);
        }
    }
}