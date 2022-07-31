using System.Threading.Tasks;
using Abp.Web.Security.AntiForgery;
using Microsoft.AspNetCore.Antiforgery;
using WorkflowDemo.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace WorkflowDemo.Web.Host.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AntiForgeryController : WorkflowDemoControllerBase
    {
        private readonly IAntiforgery _antiforgery;
        private readonly IAbpAntiForgeryManager _antiForgeryManager;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="antiforgery"></param>
        /// <param name="antiForgeryManager"></param>
        public AntiForgeryController(IAntiforgery antiforgery, IAbpAntiForgeryManager antiForgeryManager)
        {
            _antiforgery = antiforgery;
            _antiForgeryManager = antiForgeryManager;
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetCookie()
        {
            _antiForgeryManager.SetCookie(HttpContext);
        }
    }
}
