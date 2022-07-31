using System.Collections.Generic;

namespace WorkflowDemo.Authentication.External
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExternalAuthConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
