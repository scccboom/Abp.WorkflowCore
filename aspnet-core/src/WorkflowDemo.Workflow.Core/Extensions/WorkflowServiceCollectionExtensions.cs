using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkflowDemo
{
    /// <summary>
    /// 
    /// </summary>
    public static class WorkflowServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWrokflowHostedService(this IServiceCollection services)
{
            return services.AddHostedService<WorkflowHostedService>();
        }
    }
}
