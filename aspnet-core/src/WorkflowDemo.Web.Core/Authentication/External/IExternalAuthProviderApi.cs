using System.Threading.Tasks;

namespace WorkflowDemo.Authentication.External
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExternalAuthProviderApi
    {
        /// <summary>
        /// 
        /// </summary>
        ExternalLoginProviderInfo ProviderInfo { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        Task<bool> IsValidUser(string userId, string accessCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        Task<ExternalAuthUserInfo> GetUserInfo(string accessCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerInfo"></param>
        void Initialize(ExternalLoginProviderInfo providerInfo);
    }
}
