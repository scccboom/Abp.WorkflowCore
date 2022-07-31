using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using WorkflowDemo.Validation;

namespace WorkflowDemo.Authorization.Accounts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class RegisterInput : IValidatableObject
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [EmailAddress]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

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
        [DisableAuditing]
        public string CaptchaResponse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!UserName.IsNullOrEmpty())
            {
                if (!UserName.Equals(EmailAddress) && ValidationHelper.IsEmail(UserName))
                {
                    yield return new ValidationResult("Username cannot be an email address unless it's the same as your email address!");
                }
            }
        }
    }
}
