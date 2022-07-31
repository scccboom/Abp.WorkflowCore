using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace WorkflowDemo.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class WorkflowDemoControllerBase: AbpController
    {
        /// <summary>
        /// 
        /// </summary>
        protected WorkflowDemoControllerBase()
        {
            LocalizationSourceName = WorkflowDemoConsts.LocalizationSourceName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identityResult"></param>
        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
