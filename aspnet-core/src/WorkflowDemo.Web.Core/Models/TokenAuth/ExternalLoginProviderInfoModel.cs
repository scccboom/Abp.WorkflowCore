using Abp.AutoMapper;
using WorkflowDemo.Authentication.External;

namespace WorkflowDemo.Models.TokenAuth
{
    /// <summary>
    /// 
    /// </summary>
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }
    }
}
