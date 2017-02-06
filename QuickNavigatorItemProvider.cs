using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.PageExtensions;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Web;

namespace Gosso.EPiServerAddOn.QuickNavExtension
{
    [ServiceConfiguration(typeof(IQuickNavigatorItemProvider))]
    public class QuickNavigatorItemProvider : IQuickNavigatorItemProvider
    {
        private readonly IContentLoader contentLoader;

        public int SortOrder
        {
            get
            {
                return 150;
            }
        }

        public QuickNavigatorItemProvider(IContentLoader contentLoader)
        {
            this.contentLoader = contentLoader;
        }

        public IDictionary<string, QuickNavigatorMenuItem> GetMenuItems(ContentReference currentContent)
        {
            var appsetting = System.Web.Configuration.WebConfigurationManager.AppSettings["Gosso.QuickNav"] + "";
            var urlBuilder = new UrlBuilder("/logout/now");

            var dictionary = new Dictionary<string, QuickNavigatorMenuItem>();

            if (HttpContext.Current.User.IsInRole("WebAdmins"))
            {
                if (string.IsNullOrEmpty(appsetting) || appsetting.Contains("admin"))
                {
                    var editUrl = EPiServer.Editor.PageEditing.GetEditUrl(currentContent);
                    if (editUrl != null)
                    {
                        editUrl = editUrl.Replace("#", "admin/#");
                    }
                    dictionary.Add("admin", new QuickNavigatorMenuItem("/shell/cms/menu/admin", editUrl, null, "true", null));
                }

                if (appsetting.Contains("contenttype"))
                {
                    var editUrl = EPiServer.Editor.PageEditing.GetEditUrl(currentContent);

                    if (editUrl != null)
                    {
                        PageData pd = null;

                        if (this.contentLoader.TryGet<PageData>(currentContent, out pd))
                        {
                            editUrl = editUrl.Replace("#", "admin/?customdefaultpage=admin/EditContentType.aspx?typeId=" + pd.ContentTypeID + "#");
                            var n = LocalizationService.Current.GetString("/addon/quicknav/pagetype", "Admin pagetype") + " " + pd.PageTypeName;
                            dictionary.Add("EditContentType", new QuickNavigatorMenuItem(n, editUrl, null, "true", null));
                        }
                    }

                }
            }

            if (string.IsNullOrEmpty(appsetting) || appsetting.Contains("logout"))
            {
                if (this.IsPageData(currentContent))
                {
                    string url = UrlResolver.Current.GetUrl(currentContent);
                    urlBuilder.QueryCollection.Add("ReturnUrl", url);
                }

                dictionary.Add("customlogout",
                    new QuickNavigatorMenuItem("/shell/cms/menu/logout", urlBuilder.ToString(), null, "true", null)
                );
            }

            return dictionary;
        }


        private bool IsPageData(ContentReference currentContentLink)
        {
            PageData pd = null;
            if (this.contentLoader.TryGet<PageData>(currentContentLink, out pd))
                return true;
            else
                return false;
        }
    }
}