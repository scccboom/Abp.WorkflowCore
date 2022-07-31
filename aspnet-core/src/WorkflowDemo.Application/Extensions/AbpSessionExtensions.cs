using Abp.Dependency;
using Abp.Runtime.Session;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace WorkflowDemo.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class AbpSessionExtensions
    {
        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static string GetUserName(this IPrincipalAccessor accessor)
        {
            return accessor?.Principal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }
    }
}
