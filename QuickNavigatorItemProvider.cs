using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.PageExtensions;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Web;

namespace Gosso.EPiServerAddOn.QuickNavExtension
{
    [ServiceConfiguration(typeof(IQuickNavigatorItemProvider))]
    public class QuickNavigatorItemProvider : IQuickNavigatorItemProvider
    {
        private readonly IContentLoader contentLoader;
        private readonly EditUrlResolver editUrlResolver;

        public int SortOrder
        {
            get
            {
                return 150;
            }
        }

        public QuickNavigatorItemProvider(IContentLoader contentLoader, EditUrlResolver editUrlResolver)
        {
            this.contentLoader = contentLoader;
            this.editUrlResolver = editUrlResolver;
        }

        public IDictionary<string, QuickNavigatorMenuItem> GetMenuItems(ContentReference currentContent)
        {
            var appsetting = System.Web.Configuration.WebConfigurationManager.AppSettings["Gosso.QuickNav"] + "";

            string[] menuitems = new string[] { "admin", "logout" };
            if (!String.IsNullOrEmpty(appsetting))
            {
                menuitems = appsetting.Split(",".ToCharArray());
            }

            var dictionary = new Dictionary<string, QuickNavigatorMenuItem>();

            foreach (var item in menuitems)
            {
                if (!String.IsNullOrEmpty(item))
                {
                    var menu = DoMagic(item, currentContent);
                    if (menu!=null)
                    dictionary.Add(item, menu);
                }
            }



            return dictionary;
        }


        private QuickNavigatorMenuItem DoMagic(string item, ContentReference currentContent)
        {

            string keyWord = item;
            string[] arr = new string[0];
            if (item.IndexOf("|") > 0)
            {
                arr = item.Split('|');
                if (arr.Length > 2)
                {
                    if (!string.IsNullOrEmpty(arr[2]))
                    {
                        if (!HttpContext.Current.User.IsInRole(arr[2]))
                            return null;
                    }
                }
                keyWord = arr[0];
            }

            if (keyWord == "imagevault")
            {
                var vaulturl = GetEditUrl() + UriSupport.ResolveUrlFromUIBySettings("../ImageVault.EPiServer.UI/ImageVaultUi");
                return new QuickNavigatorMenuItem("Imagevault", vaulturl, null, "true", null);

            }

            if (keyWord == "find")
            {
                var find = GetEditUrl() + UriSupport.ResolveUrlFromUIBySettings("../find/");
                return new QuickNavigatorMenuItem("Find", find, null, "true", null);
            }

            
            if (keyWord == "admin")
            {
                if (!HttpContext.Current.User.IsInRole("WebAdmins"))
                    return null;

                var editUrl = GetEditUrl() + EPiServer.Editor.PageEditing.GetEditUrl(currentContent);
                editUrl = editUrl.Replace("#", "admin/default.aspx#");

                return new QuickNavigatorMenuItem("/shell/cms/menu/admin", editUrl, null, "true", null);
            }

            if (keyWord == "contenttype")
            {
                if (!HttpContext.Current.User.IsInRole("WebAdmins"))
                    return null;

                var editUrl = GetEditUrl() + EPiServer.Editor.PageEditing.GetEditUrl(currentContent);

                PageData pd = null;

                if (this.contentLoader.TryGet<PageData>(currentContent, out pd))
                {
                    editUrl = editUrl.Replace("#", "admin/default.aspx?customdefaultpage=admin/EditContentType.aspx?typeId=" + pd.ContentTypeID + "#");
                    var n = LocalizationService.Current.GetString("/addon/quicknav/pagetype", "Admin pagetype") + " " + pd.PageTypeName;
                    return new QuickNavigatorMenuItem(n, editUrl, null, "true", null);
                }
                return null;
            }            

            if (keyWord == "logout")
            {
                var urlBuilder = new UrlBuilder("/logout/now");

                if (this.IsPageData(currentContent))
                {

                    string url = UrlResolver.Current.GetUrl(currentContent);
                    urlBuilder.QueryCollection.Add("ReturnUrl", url);
                }

                return new QuickNavigatorMenuItem("/shell/cms/menu/logout", urlBuilder.ToString(), null, "true", null);
            }

            if (arr.Length > 1)
            {
                return new QuickNavigatorMenuItem(LocalizationService.Current.GetString(arr[0], arr[0]), arr[1], null, "true", null);
            }            

            //oh no... 
            return new QuickNavigatorMenuItem(LocalizationService.Current.GetString(item, item), "javascript:alert('Wrong config in Appsetting Gosso.QuickNav')", null, "true", null);
        }

        private string GetEditUrl()
        {
            Url editViewUrl = editUrlResolver.GetEditViewUrl(new EditUrlArguments()
            {
                ForceEditHost = true
            });
            return editViewUrl?.Uri.ToString().Replace(editViewUrl.Path, "");//Just want the HOST to *EDIT*
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