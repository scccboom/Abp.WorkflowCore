using Abp.Dependency;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using WorkflowCore.Interface;

using WorkflowDemo.Workflows;

namespace WorkflowDemo
{
    /// <summary>
    /// 
    /// </summary>
    public static class WorkflowMiddlewareExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="useHostApplictionLifeTime"></param>
        /// <returns></returns>
        public static void UseWorkflow(this IApplicationBuilder app, bool useHostApplictionLifeTime = false)
        {
            app.ApplicationServices.GetRequiredService<AbpStepBodyDefinitionContext>().Initialize();

            app.ApplicationServices.GetRequiredService<AbpWorkflowManager>().Initialize();

            if (useHostApplictionLifeTime)
            {
                var host = app.ApplicationServices.GetRequiredService<IWorkflowHost>();
                host.Start();
                IHostApplicationLifetime service = app.ApplicationServices.GetService<IHostApplicationLifetime>();
                service.ApplicationStopping.Register(()=>
                {
                    host.Stop();
                });
            }
        }
    }
}
