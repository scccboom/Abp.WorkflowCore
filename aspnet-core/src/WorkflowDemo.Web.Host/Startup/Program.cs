using Abp.AspNetCore.Dependency;
using Abp.Dependency;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

using System.IO;

namespace WorkflowDemo.Web.Host.Startup
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            BuildWebHost(args).Build().Run();
        }

        private static IHostBuilder BuildWebHost(string[] args)
        {
            return Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseStartup<Startup>();
                })
                .UseCastleWindsor(IocManager.Instance.IocContainer);
        }
    }
}
