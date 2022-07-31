using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;

namespace WorkflowDemo.Authorization.Accounts.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class IsTenantAvailableInput
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [StringLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}
