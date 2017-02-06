using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using System.Web.Routing;

namespace Gosso.EPiServerAddOn.QuickNavExtension
{
    [InitializableModule]
    public class QuickNavigatorInitializer : IInitializableModule
    {
        private const string RouteName = "LogoutRedirect";

        public void Initialize(InitializationEngine context)
        {
            //important to override with a "key" (that can be anything), not to cause it to be a default route and the MVC will use default when used with @Ajax.ActionLink and sometimes xforms action url
            RouteTable.Routes.Add(RouteName, new Route("logout/{key}", new RedirectRouteHandler()));
        }

        public void Preload(string[] parameters) { }

        public void Uninitialize(InitializationEngine context)
        {
            RouteTable.Routes.Remove(RouteTable.Routes[RouteName]);
        }
    }
}