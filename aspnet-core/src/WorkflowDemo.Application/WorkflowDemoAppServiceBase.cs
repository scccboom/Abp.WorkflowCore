using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Abp.Application.Services;
using Abp.IdentityFramework;
using Abp.Runtime.Session;
using WorkflowDemo.Authorization.Users;
using WorkflowDemo.MultiTenancy;

namespace WorkflowDemo
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class WorkflowDemoAppServiceBase : ApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        public TenantManager TenantManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserManager UserManager { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected WorkflowDemoAppServiceBase()
        {
            LocalizationSourceName = WorkflowDemoConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        protected virtual async Task<User> GetCurrentUserAsync()
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            if (user == null)
            {
                throw new Exception("There is no current user!");
            }

            return user;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual Task<Tenant> GetCurrentTenantAsync()
        {
            return TenantManager.GetByIdAsync(AbpSession.GetTenantId());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityResult"></param>
        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
