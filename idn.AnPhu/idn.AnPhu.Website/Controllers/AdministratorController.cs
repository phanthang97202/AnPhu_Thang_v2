using Client.Core.Configuration;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Utils;
using idn.AnPhu.Website.Extensions;
using idn.AnPhu.Website.Filters;
using idn.AnPhu.Website.Security;
using idn.AnPhu.Website.State;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace idn.AnPhu.Website.Controllers
{
    //[CustomAuthorize]
    [CustomAuthorizeAttribute(UserRole.Moderator)]
    public class AdministratorController : BaseController
    {
        private string FullName = "";
        private string Avatar = "";
        public int PageSizeAdminConfig = 0;

        public string Culture
        {
            get
            {
                return System.Globalization.CultureInfo.CurrentCulture.ToString();
            }
        }

        public SiteConfiguration Config
        {
            get
            {
                return SiteConfiguration.Current;
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var isCulture = false;
            string culture = (string)RouteData.Values["CultureName"];
            if (CUtils.IsNullOrEmpty(culture))
            {
                culture = (string)RouteData.Values["Culture"];
                if (!CUtils.IsNullOrEmpty(culture))
                {

                    System.Web.HttpContext.Current.Session["Culture"] = culture;
                    var cList = Client.Core.Configuration.SiteConfiguration.Current.AcceptedCultures;
                    foreach (var c in cList)
                    {
                        if (c.TwoLetterISOLanguageName.Equals(culture, StringComparison.InvariantCultureIgnoreCase))
                        {
                            culture = c.TwoLetterISOLanguageName;
                            isCulture = true;
                            break;
                        }
                    }
                }
            }

            if (CUtils.IsNullOrEmpty(culture) || !isCulture)
            {
                culture = SiteConfiguration.Current.DefaultCultureName;
                if (!CUtils.IsNullOrEmpty(culture))
                {
                    culture = culture.Substring(0, 2);
                }
                System.Web.HttpContext.Current.Session["Culture"] = culture;
            }

            CultureInfo ci = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
        }

        #region State management
        private SessionWrapper _session;
        public new SessionWrapper Session
        {
            get { return _session ?? (_session = new SessionWrapper(ControllerContext.HttpContext.Session)); }
            set
            {
                _session = value;
            }
        }

        private CacheWrapper _cache;
        public CacheWrapper Cache
        {
            get { return _cache ?? (_cache = new CacheWrapper(this.ControllerContext.HttpContext.Cache)); }
            set
            {
                _cache = value;
            }
        }

        /// <summary>
        /// Gets the current user in session
        /// </summary>
        public new UserState UserState
        {
            get
            {
                return Session.UserState;
            }
        }

        //public UserRole? Role
        //{
        //    get
        //    {
        //        UserRole? role = null;
        //        if (HttpContext != null && Session.UserState != null)
        //        {
        //            role = Session.UserState.Role;
        //        }
        //        return role;
        //    }
        //}

        #endregion

        #region Init
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            this.Init();
        }

        protected virtual void Init()
        {
            var bUserStateNotNull = false;
            if (Session.UserState == null)
            {
                if (!SecurityHelper.TryLoginFromProviders(Session, Cache, HttpContext))
                {
                    bUserStateNotNull = false;
                    FormsAuthentication.SignOut();

                    string action = this.RouteData.Values["action"].ToString();

                    if (!action.Equals("Login", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var rawUrl = Request.RawUrl;
                        var culture = CUtils.StrValue(RouteData.Values["Culture"]);
                        var isCulture = false;
                        if (!CUtils.IsNullOrEmpty(culture))
                        {
                            var cList = Client.Core.Configuration.SiteConfiguration.Current.AcceptedCultures;
                            foreach (var c in cList)
                            {
                                if (c.TwoLetterISOLanguageName.Equals(culture, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    isCulture = true;
                                    culture = c.TwoLetterISOLanguageName;
                                    System.Web.HttpContext.Current.Session["Culture"] = culture;
                                    break;
                                }
                            }
                        }
                        if (!isCulture)
                        {
                            culture = SiteConfiguration.Current.DefaultCultureName;
                            if (!CUtils.IsNullOrEmpty(culture))
                            {
                                culture = culture.Substring(0, 2);
                            }
                            RouteData.Values["Culture"] = culture;
                            rawUrl = Url.ActionLocalized("Index", "Home", new { area = "Auth" });
                        }
                        var redirect = Url.ActionLocalized("Login", "Account", new { area = "Auth", returnUrl = rawUrl });
                        //redirect = Url.Action("Login", "Account", new { area = "Auth", returnUrl = Request.RawUrl });
                        Response.Redirect(redirect);
                    }
                }
                else
                {
                    bUserStateNotNull = true;
                }
            }
            else
            {
                bUserStateNotNull = true;
            }

            if (bUserStateNotNull)
            {
                var sysUser = Session.UserState.SysUser;

                if (sysUser != null)
                {
                    if (CUtils.IsNullOrEmpty(FullName))
                    {
                        if (!CUtils.IsNullOrEmpty(sysUser.FullName))
                        {
                            FullName = CUtils.StrTrim(sysUser.FullName);
                        }
                    }

                    if (CUtils.IsNullOrEmpty(Avatar))
                    {
                        var avatar = Url.Content("~/Areas/Auth/Content/assets/images/avatars/avatar2.png");
                        Avatar = !CUtils.IsNullOrEmpty(sysUser.Avatar) ? CUtils.StrTrim(sysUser.Avatar) : avatar;
                    }
                }
                ViewBag.Fullname = FullName;
                ViewBag.Avatar = Avatar;
            }

            if (PageSizeAdminConfig == 0)
            {
                var pageSizeAdminConfig = System.Configuration.ConfigurationManager.AppSettings["PageSizeAdminConfig"];
                if (!CUtils.IsNullOrEmpty(pageSizeAdminConfig))
                {
                    PageSizeAdminConfig = CUtils.ConvertToInt32(pageSizeAdminConfig);
                }
                else
                {
                    PageSizeAdminConfig = 10;
                }
            }

        }

        #endregion

    }
}