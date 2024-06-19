using Client.Core.Configuration;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Utils;
using idn.AnPhu.Website.Controllers;
using idn.AnPhu.Website.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
//using System.Web.Http;

namespace idn.AnPhu.Website.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Required minimal User role
        /// </summary>
        public UserRole[] UserRoles
        {
            get;
            set;
        }

        public RouteCollection Routes
        {
            get;
            set;
        }

        public CustomAuthorizeAttribute()
        {
            this.Routes = RouteTable.Routes;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRole">Required minimal role to access</param>
        public CustomAuthorizeAttribute(params UserRole[] userRole)
            : this()
        {
            this.UserRoles = userRole;
        }

        /// <summary>
        /// Determines if the filter must return forbidden status in the case the user is not logged in
        /// </summary>
        public bool RefuseOnFail
        {
            get;
            set;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            SessionWrapper session = new SessionWrapper(httpContext.Session);
            //return true;
            return IsAuthorized(session.UserState);
        }

        /// <summary>
        /// Determines if a user is authorized
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected virtual bool IsAuthorized(UserState user)
        {
            var bRoles = false;
            if (user == null)
            {
                return bRoles;
            }
            if (this.UserRoles != null && this.UserRoles.Count() != 0)
            {
                if(user.Roles != null && user.Roles.Count > 0)
                {
                    foreach (UserRole ur in UserRoles)
                    {
                        if (user.Role == UserRole.SysAdmin)
                        {
                            bRoles = true;
                            return true;
                        }
                        if (user.Role == ur)
                        {
                            bRoles = true;
                            return true;
                        }
                    }
                }
            }
            //bRoles = true;
            return bRoles;
        }

        /// <summary>
        /// Handles the request when the user is not authorized
        /// </summary>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var culture = CUtils.StrValue(System.Web.HttpContext.Current.Session["Culture"]);
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
                System.Web.HttpContext.Current.Session["Culture"] = culture;
                
            }
            string redirectOnSuccess = filterContext.HttpContext.Request.Url.PathAndQuery;
            VirtualPathData path = this.Routes.GetVirtualPath(filterContext.RequestContext, new RouteValueDictionary(new
            {
                culture = culture,
                controller = "Account",
                action = "Login",
                returnUrl = "",
                //role = this.UserRole
            }));
            string loginUrl = path.VirtualPath;
            //var redirect = Url.ActionLocalized("Login", "Account", new { area = "Auth", returnUrl = rawUrl });

            filterContext.Result = new RedirectResult(loginUrl, false);

            //if (RefuseOnFail)
            //{
            //    filterContext.Result = ResultHelper.ForbiddenResult(filterContext.Controller as AdministratorController);
            //}
            //else
            //{
            //    string redirectOnSuccess = filterContext.HttpContext.Request.Url.PathAndQuery;
            //    VirtualPathData path = this.Routes.GetVirtualPath(filterContext.RequestContext, new RouteValueDictionary(new
            //    {
            //        controller = "Authentication",
            //        action = "Login",
            //        returnUrl = redirectOnSuccess,
            //        role = this.UserRole
            //    }));
            //    if (path == null)
            //    {
            //        throw new ArgumentException("Route for Authentication>Login not found.");
            //    }
            //    string loginUrl = path.VirtualPath;
            //    filterContext.Result = new RedirectResult(loginUrl, false);
            //}
        }
    }
}