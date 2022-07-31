using System.Threading.Tasks;
using Abp.Dependency;

namespace WorkflowDemo.Authentication.External
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ExternalAuthProviderApiBase : IExternalAuthProviderApi, ITransientDependency
    {
        /// <summary>
        /// 
        /// </summary>
        public ExternalLoginProviderInfo ProviderInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerInfo"></param>
        public void Initialize(ExternalLoginProviderInfo providerInfo)
        {
            ProviderInfo = providerInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        public async Task<bool> IsValidUser(string userId, string accessCode)
        {
            var userInfo = await GetUserInfo(accessCode);
            return userInfo.ProviderKey == userId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        public abstract Task<ExternalAuthUserInfo> GetUserInfo(string accessCode);
    }
}
