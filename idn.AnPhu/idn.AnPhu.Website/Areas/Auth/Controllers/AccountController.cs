using Client.Core.Data.Entities.Validation;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Services;
using idn.AnPhu.Utils;
using idn.AnPhu.Website.Controllers;
using idn.AnPhu.Website.Extensions;
using idn.AnPhu.Website.Filters;
using idn.AnPhu.Website.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace idn.AnPhu.Website.Areas.Auth.Controllers
{
    [Authorize]
    [CustomAuthorizeAttribute(UserRole.Moderator)]
    public class AccountController : AdministratorController
    {
        private Sys_UserManager Sys_UserManager
        {
            get { return ServiceFactory.Sys_UserManager; }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //if (Session != null)
            //{
            //    Session.UserState = null;
            //    Session.SessionId = null;
            //    Session.Session.Clear();
            //}
            //Response.Cookies.Clear();
            //if (CUtils.IsNullOrEmpty(returnUrl))
            //{
            //    returnUrl = Url.ActionLocalized("Index", "Home", new { area = "Auth" });
            //}
            //ViewBag.ReturnUrl = returnUrl;
            //return View();

            // ThangPV edit 
            ViewBag.IsLogin = true;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //[RequireAuthorization(UserRole.Admin)]
        //[HttpGet]
        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            var resultEntry = new JsonResultEntry() { Success = false };
            try
            {
                if (Session != null)
                {
                    Session.UserState = null;
                    Session.SessionId = null;
                    Session.Session.Clear();
                }
                var abc = Sys_UserManager.Login(model.UserName, model.Password);
                if (ModelState.IsValid && Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (CUtils.IsNullOrEmpty(returnUrl) || returnUrl == "/")
                    {
                        var url = Url.ActionLocalized("Index", "Home", new { area = "Auth" });
                        returnUrl = url;
                    }
                    resultEntry.Success = true;
                    resultEntry.RedirectToOpener = true;
                    resultEntry.RedirectUrl = returnUrl;
                }
                else
                {
                    var exitsData = "Tên đăng nhập hoặc mật khẩu không hợp lệ!";
                    resultEntry.AddMessage(exitsData);
                }


                return Json(resultEntry);
                // If we got this far, something failed, redisplay form
                //ModelState.AddModelError("", "The user name or password provided is incorrect.");
                //return View(model);
            }
            catch (ValidationException valEx)
            {
                foreach (var ver in valEx.ValidationErrors)
                {
                    resultEntry.AddFieldError(ver.FieldName, ver.ErrorMessage);
                }
            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(this, e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);

        }

        public ActionResult LogOff()
        {
            Session.UserState = null;
            Session.SessionId = null;
            Response.Cookies.Clear();
            FormsAuthentication.SignOut();

            var c = new HttpCookie("ASP.NET_SessionId") { Expires = DateTime.Now.AddDays(-1) };
            Response.Cookies.Add(c);

            Session.Session.Clear();
            var url = Url.Action("Index", "Home", new { area = "" });
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                //return RedirectToAction("Index", "Home");
                return RedirectToAction("Index", "Home", new { area = "Auth" });
            }
        }
    }
}