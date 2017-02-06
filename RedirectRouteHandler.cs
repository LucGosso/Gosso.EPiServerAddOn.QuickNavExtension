using EPiServer.Util;
using System;
using System.Web;
using System.Web.Compilation;
using System.Web.Routing;

namespace Gosso.EPiServerAddOn.QuickNavExtension
{
    public class RedirectRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            var logout = BuildManager.CreateInstanceFromVirtualPath("/Util/Logout.aspx", typeof(Logout)) as Logout;
            if (logout == null)
            {
                // Something seems wrong
                throw new Exception();
            }

            logout.PreRender += this.LogoutOnPreRender;
            return logout;
        }

        private void LogoutOnPreRender(object sender, EventArgs eventArgs)
        {
            var returnUrl = HttpContext.Current.Request.QueryString["ReturnUrl"];
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = "/";
            HttpContext.Current.Response.Redirect(returnUrl);
        }
    }
}