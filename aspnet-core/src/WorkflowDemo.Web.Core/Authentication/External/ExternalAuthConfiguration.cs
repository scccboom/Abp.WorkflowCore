using System.Collections.Generic;
using Abp.Dependency;

namespace WorkflowDemo.Authentication.External
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalAuthConfiguration : IExternalAuthConfiguration, ISingletonDependency
    {
        /// <summary>
        /// 
        /// </summary>
        public List<ExternalLoginProviderInfo> Providers { get; }

        /// <summary>
        /// 
        /// </summary>
        public ExternalAuthConfiguration()
        {
            Providers = new List<ExternalLoginProviderInfo>();
        }
    }
}
