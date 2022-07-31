using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using WorkflowDemo.Configuration;

namespace WorkflowDemo.Web.Host.Startup
{
    /// <summary>
    /// 
    /// </summary>
    [DependsOn(typeof(WorkflowDemoWebCoreModule))]
    public class WorkflowDemoWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public WorkflowDemoWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(WorkflowDemoWebHostModule).GetAssembly());
        }
    }
}
