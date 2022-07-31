using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace WorkflowDemo.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetAppConfiguration(this IWebHostEnvironment env)
        {
            return AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
        }
    }
}
