using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Dependency;

namespace WorkflowDemo.Authentication.External
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalAuthManager : IExternalAuthManager, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iocResolver"></param>
        /// <param name="externalAuthConfiguration"></param>
        public ExternalAuthManager(IIocResolver iocResolver, IExternalAuthConfiguration externalAuthConfiguration)
        {
            _iocResolver = iocResolver;
            _externalAuthConfiguration = externalAuthConfiguration;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="providerKey"></param>
        /// <param name="providerAccessCode"></param>
        /// <returns></returns>
        public Task<bool> IsValidUser(string provider, string providerKey, string providerAccessCode)
        {
            using (var providerApi = CreateProviderApi(provider))
            {
                return providerApi.Object.IsValidUser(providerKey, providerAccessCode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="accessCode"></param>
        /// <returns></returns>
        public Task<ExternalAuthUserInfo> GetUserInfo(string provider, string accessCode)
        {
            using (var providerApi = CreateProviderApi(provider))
            {
                return providerApi.Object.GetUserInfo(accessCode);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IDisposableDependencyObjectWrapper<IExternalAuthProviderApi> CreateProviderApi(string provider)
        {
            var providerInfo = _externalAuthConfiguration.Providers.FirstOrDefault(p => p.Name == provider);
            if (providerInfo == null)
            {
                throw new Exception("Unknown external auth provider: " + provider);
            }

            var providerApi = _iocResolver.ResolveAsDisposable<IExternalAuthProviderApi>(providerInfo.ProviderApiType);
            providerApi.Object.Initialize(providerInfo);
            return providerApi;
        }
    }
}
