using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Castle.MicroKernel.Registration;
using Castle.Windsor.MsDependencyInjection;
using Abp.Dependency;
using WorkflowDemo.EntityFrameworkCore;
using WorkflowDemo.Identity;
using WorkflowCore.Interface;
using WorkflowDemo.Workflows;

namespace WorkflowDemo.Tests.DependencyInjection
{
    public static class ServiceCollectionRegistrar
    {
        public static void Register(IIocManager iocManager)
        {
            var services = new ServiceCollection();

            IdentityRegistrar.Register(services);

            services.AddEntityFrameworkInMemoryDatabase();

            services.AddSingleton<IPersistenceProvider, AbpPersistenceProvider>();
            services.AddWorkflow(options =>
            {
                options.UsePersistence(sp => sp.GetService<AbpPersistenceProvider>());
            });
            services.AddWorkflowDSL();

            var serviceProvider = WindsorRegistrationHelper.CreateServiceProvider(iocManager.IocContainer, services);

            var builder = new DbContextOptionsBuilder<WorkflowDemoDbContext>();
            builder.UseInMemoryDatabase(Guid.NewGuid().ToString()).UseInternalServiceProvider(serviceProvider);

            iocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<WorkflowDemoDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton()
            );
        }
    }
}
