using Client.Core.Configuration;
using idn.AnPhu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace idn.AnPhu.Website.Extensions
{
    public static class UrlExtensions
    {


        public static string LocalizedUrl(this string url)
        {
            var culture = CUtils.StrValue(System.Web.HttpContext.Current.Session["Culture"]);
            if (CUtils.IsNullOrEmpty(culture))
            {
                culture = SiteConfiguration.Current.DefaultCultureName;
                if (!CUtils.IsNullOrEmpty(culture))
                {
                    culture = culture.Substring(0, 2);
                }
            }
            if (CUtils.IsNullOrEmpty(culture))
            {
                culture = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            }
            var cList = Client.Core.Configuration.SiteConfiguration.Current.AcceptedCultures;

            if (url.StartsWith("/" + culture)) return url;


            foreach (var dc in cList)
            {
                if (url.StartsWith("/" + dc.TwoLetterISOLanguageName))
                {
                    url = url.Replace("/" + dc.TwoLetterISOLanguageName, "/");
                    break;
                }
            }


            if (url.Equals("/") && culture.Equals("vi", StringComparison.InvariantCultureIgnoreCase))
            {
                return url;

            }
            return string.Format("/{0}{1}", culture, url);
        }



        public static string ActionLocalized(this UrlHelper url, string actionName)
        {

            return url.Action(actionName.ToLower()).LocalizedUrl();

        }

        public static string ActionLocalized(this UrlHelper url, string actionName, object routeValues)
        {

            return url.Action(actionName.ToLower(), routeValues).LocalizedUrl();
        }

        public static string ActionLocalized(this UrlHelper url, string actionName, System.Web.Routing.RouteValueDictionary routeValues)
        {


            return url.Action(actionName.ToLower(), routeValues).LocalizedUrl();
        }




        public static string ActionLocalized(this UrlHelper url, string actionName, string controllerName)
        {

            return url.Action(actionName.ToLower(), controllerName.ToLower()).LocalizedUrl();

        }


        public static string ActionLocalized(this UrlHelper url, string actionName, string controllerName, object routeValues)
        {

            return url.Action(actionName.ToLower(), controllerName.ToLower(), routeValues).LocalizedUrl();

        }

        public static string ActionLocalized(this UrlHelper url, string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues)
        {


            return url.Action(actionName.ToLower(), controllerName.ToLower(), routeValues).LocalizedUrl();

        }



        public static string ActionLocalized(this UrlHelper url, string actionName, string controllerName, object routeValues, string protocol)
        {


            return url.Action(actionName.ToLower(), controllerName.ToLower(), routeValues, protocol).LocalizedUrl();

        }


        public static string ActionLocalized(this UrlHelper url, string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, string protocol)
        {


            return url.Action(actionName.ToLower(), controllerName.ToLower(), routeValues, protocol).LocalizedUrl();

        }

        public static string ActionLocalized(this UrlHelper url, string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeValues, string protocol, string hostName)
        {


            return url.Action(actionName.ToLower(), controllerName.ToLower(), routeValues, protocol, hostName).LocalizedUrl();

        }

    }
}