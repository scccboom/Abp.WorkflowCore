using System.Threading.Tasks;

namespace WorkflowDemo.Authentication.External
{
    /// <summary>
    /// 
    /// </summary>
    public interface IExternalAuthManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerKey"></param>
        /// <param name="providerAccessCode"></param>
        /// <returns></returns>
        Task<bool> IsValidUser(string provider, string providerKey, string providerAccessCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        Task<ExternalAuthUserInfo> GetUserInfo(string provider, string accessCode);
    }
}
