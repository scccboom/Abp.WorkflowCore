using System;
using Microsoft.IdentityModel.Tokens;

namespace WorkflowDemo.Authentication.JwtBearer
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenAuthConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        public SymmetricSecurityKey SecurityKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan Expiration { get; set; }
    }
}
