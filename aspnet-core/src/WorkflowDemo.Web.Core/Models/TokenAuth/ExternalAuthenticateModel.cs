using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace WorkflowDemo.Models.TokenAuth
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalAuthenticateModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(UserLogin.MaxLoginProviderLength)]
        public string AuthProvider { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(UserLogin.MaxProviderKeyLength)]
        public string ProviderKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string ProviderAccessCode { get; set; }
    }
}
