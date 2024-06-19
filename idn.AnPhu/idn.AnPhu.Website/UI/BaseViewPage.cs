using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Client.Core.Extensions;
using Client.Core.Localization;
using idn.AnPhu.Utils;
using idn.AnPhu.Website.State;

namespace idn.AnPhu.Website.UI
{
    public class BaseViewPage<TModel> : WebViewPage<TModel> where TModel : class
    {
        #region Initialization
        public BaseViewPage()
        {

        }

        protected override void InitializePage()
        {
            var cultureName = Culture;
            Localizer = Localizer.GetCurrent(cultureName);
            base.InitializePage();
        }
        #endregion

        #region Properties
        private SessionWrapper _session;
        public new SessionWrapper Session
        {
            get
            {
                if (_session == null && Context != null)
                {
                    _session = new SessionWrapper(Context.Session);
                }
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        private CacheWrapper _cache;
        public new CacheWrapper Cache
        {
            get
            {
                if (_cache == null && Context != null)
                {
                    _cache = new CacheWrapper(Context.Cache);
                }
                return _cache;
            }
            set
            {
                _cache = value;
            }
        }

        public virtual Localizer Localizer
        {
            get;
            set;
        }

        public UserState UserState { get { return Session.UserState; } }


        public string ControllerName
        {
            get
            {
                return ViewContext.RouteData.GetRequiredString("controller");
            }
        }

        public string ActionName
        {
            get
            {
                return ViewContext.RouteData.GetRequiredString("action");
            }
        }



        #region Domain

        public string Domain
        {
            get
            {
                if (this.ViewContext == null)
                {
                    return "http://www.idocnet.com";
                }
                return this.ViewContext.HttpContext.Request.Url.Scheme + Uri.SchemeDelimiter + this.ViewContext.HttpContext.Request.Url.Host;
            }
        }
        #endregion

        /// <summary>
        /// Gets the current action name
        /// </summary>

        /// <summary>
        /// Determines if it is the current actionName
        /// </summary>
        /// <param name="actionName"></param>
        /// <returns></returns>
        public bool IsAction(string actionName)
        {
            return this.ActionName.ToUpper() == actionName.ToUpper();
        }
        #endregion

        #region Methods
        protected ViewDataDictionary CreateViewData(object values)
        {
            return ViewDataExtensions.CreateViewData(values);
        }

        /// <summary>
        /// Returns an HtmlString containing the localized value
        /// </summary>
        /// <param name="neutralValue">The text to be localized</param>
        public virtual IHtmlString T(string neutralValue)
        {
            return S(neutralValue).ToHtmlString();
        }

        /// <summary>
        /// Returns an HtmlString containing the localized value
        /// </summary>
        /// <param name="neutralValue">The text to be localized</param>
        public virtual IHtmlString T(string neutralValue, params object[] args)
        {
            return S(neutralValue, args).ToHtmlString();
        }


        /// <summary>
        /// Returns the localized representation of the string
        /// </summary>
        /// <param name="neutralValue">The text to be localized</param>
        public virtual string S(string neutralValue, params object[] args)
        {
            var text = neutralValue;
            if (Localizer != null)
            {
                text = Localizer.Get(neutralValue, args);
            }
            return text;
        }

        /// <summary>
        /// Returns the localized representation of the string
        /// </summary>
        /// <param name="neutralValue">The text to be localized</param>
        public virtual string S(string neutralValue)
        {
            var text = neutralValue;
            if (Localizer != null)
            {
                text = Localizer[neutralValue];
            }
            return text;
        }

        public override void Execute()
        {

        }

        public string GetRandomClientId()
        {
            return "cl_" + Guid.NewGuid().ToString("N").Substring(0, 6);
        }



        #endregion

        #region security

        public bool IsFunctionActive(params string[] functionNames)
        {
            if (functionNames == null || functionNames.Length == 0 || (functionNames.Length == 1 && CUtils.IsNullOrEmpty(functionNames[0]))) return true;
            if (UserState == null) return false;


            bool active = false;
            //if (UserState.ListAccess != null)
            //{
            //    active = true;
            //    foreach (string f in functionNames)
            //    {
            //        bool contains = false;
            //        foreach (var a in UserState.ListAccess)
            //        {
            //            if (a.OBJECTCODE.Equals(f.Trim(), StringComparison.InvariantCultureIgnoreCase))
            //            {
            //                contains = true;
            //                break;
            //            }
            //        }

            //        if (!contains)
            //        {
            //            active = false;
            //            break;
            //        }
            //    }
            //}

            return active;
        }


        public bool IsFunctionActiveOr(params string[] functionNames)
        {
            if (functionNames == null || functionNames.Length == 0 || (functionNames.Length == 1 && CUtils.IsNullOrEmpty(functionNames[0]))) return true;

            if (UserState != null && UserState.SysUser != null)
            {
                if (UserState.IsSysAdmin)
                {
                    return true;
                }
                else
                {
                    //foreach (string f in functionNames)
                    //{
                    //    foreach (var a in UserState.ListAccess)
                    //    {
                    //        if (a.OBJECTCODE.Equals(f.Trim(), StringComparison.InvariantCultureIgnoreCase))
                    //        {
                    //            return true;
                    //        }
                    //    }
                    //}
                }

            }

            return false;
        }



        #endregion


        #region helpers

        public HtmlString RenderMainMenuItem(string functionName, string text, string actionName, string controllerName = null, object routeValues = null, object htmlAtts = null)
        {

            string format = ""; // @"<li><a href=""{URL}"" {ATTR}>{TEXT}</a></li>";
            format += @"<li class=''>";
            format += @"<a href=""{URL}"" {ATTR} ><span class='menu-text'>{TEXT}</span></a>";
            format += @"</li>";

            if (ControllerName.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase))
            {
                return RenderItemFormat(functionName, format, text, actionName, controllerName, routeValues, htmlAtts, "active");
            }

            var renderItemHtml = RenderItemFormat(functionName, format, text, actionName, controllerName, routeValues, htmlAtts);
            return renderItemHtml;
        }



        public HtmlString RenderSubMenuItem(string functionName, string text, string actionName, string controllerName = null, object routeValues = null, object htmlAtts = null)
        {
            string format = @"<li><a href=""{URL}"" {ATTR}>{TEXT}</a></li>";


            if (ControllerName.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase) && ActionName.Equals(actionName, StringComparison.InvariantCultureIgnoreCase))
            {
                return RenderItemFormat(functionName, format, text, actionName, controllerName, routeValues, htmlAtts, "active");
            }

            return RenderItemFormat(functionName, format, text, actionName, controllerName, routeValues, htmlAtts);
        }



        public HtmlString RenderLink(string functionName, string text, string actionName, string controllerName = null, object routeValues = null, object htmlAtts = null)
        {
            string format = @"<a href=""{URL}"" {ATTR}>{TEXT}</a>";
            return RenderItemFormat(functionName, format, text, actionName, controllerName, routeValues, htmlAtts);
        }


        public HtmlString RenderItemFormat(string functionName, string format, string text = null, string actionName = null, string controllerName = null, object routeValues = null, object htmlAtts = null)
        {

            return RenderItemFormat(functionName, format, text, actionName, controllerName, routeValues, htmlAtts, "");
            //return ret;
        }



        private HtmlString RenderItemFormat(string functionName, string format, string text, string actionName, string controllerName, object routeValues, object htmlAtts, string className)
        {
            string ret = "";


            if (!CUtils.IsNullOrEmpty(functionName))
            {
                var functions = functionName.Split(',');
                if (!IsFunctionActiveOr(functions))
                {
                    return new HtmlString(ret);
                }
            }
            string atts = "";


            if (htmlAtts != null)
            {

                foreach (System.Reflection.PropertyInfo fi in htmlAtts.GetType().GetProperties())
                {
                    string attName = fi.Name;
                    object attVal = fi.GetValue(htmlAtts, null);

                    if (attName.Equals("class", StringComparison.InvariantCultureIgnoreCase))
                    {
                        className = string.Format("{0} {1}", attVal, className);
                    }
                    else
                    {
                        atts += string.Format(" {0}=\"{1}\"", attName, attVal);
                    }
                }
            }


            atts = string.Format("class=\"{0}\"{1}", className, atts);
            string url = Url.Action(actionName, controllerName, routeValues);
            string linkText = CUtils.IsNullOrEmpty(text) ? url : text;
            ret = format.Replace(@"{URL}", url).Replace(@"{ATTR}", atts).Replace(@"{TEXT}", linkText);




            return new HtmlString(ret);
            //return ret;
        }

        #endregion
    }

    public class BaseViewPage : BaseViewPage<object>
    {

    }
}