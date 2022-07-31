using System;

namespace WorkflowDemo.Authentication.External
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalLoginProviderInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Type ProviderApiType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="providerApiType"></param>
        public ExternalLoginProviderInfo(string name, string clientId, string clientSecret, Type providerApiType)
        {
            Name = name;
            ClientId = clientId;
            ClientSecret = clientSecret;
            ProviderApiType = providerApiType;
        }
    }
}
