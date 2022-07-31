using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;

namespace WorkflowDemo.Models.TokenAuth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticateModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string UserNameOrEmailAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RememberClient { get; set; }
    }
}
