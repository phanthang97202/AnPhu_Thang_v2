using Client.Core.Data.Entities.Paging;
using Client.Core.Utils;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Services;
using idn.AnPhu.Utils;
using idn.AnPhu.Website.Controllers;
using idn.AnPhu.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace idn.AnPhu.Website.Areas.Auth.Controllers
{
    public class Sys_UserController : AdministratorController// : Controller
    {
        private Sys_UserManager Sys_UserManager
        {
            get { return ServiceFactory.Sys_UserManager; }
        }

        private Sys_GroupManager Sys_GroupManager
        {
            get { return ServiceFactory.Sys_GroupManager; }
        }

        //
        // GET: /Auth/Sys_User/

        public ActionResult Index(string usercode = "", string fullname = "", string birthdayfrom = "", string birthdayto = "", string createdtimefrom = "", string createdtimeto = "", string email = "", string flagactive = "", string init = "init", int page = 0)
        {
            var startCount = "0";
            var pageInfo = new PageInfo<Sys_User>(0, PageSizeAdminConfig);
            if (init != "init")
            {
                var phoneno = "";
                var sex = "";
                var issysadmin = "";
                pageInfo = Sys_UserManager.Search(usercode, fullname, birthdayfrom, birthdayto, phoneno, email, sex, flagactive, issysadmin, page, PageSizeAdminConfig);
                if (pageInfo != null && pageInfo.DataList != null && pageInfo.DataList.Count > 0)
                {
                    startCount = (page * PageSizeAdminConfig).ToString();
                }
            }
            else
            {
                createdtimefrom = CUtils.GetDateToSearch(DateTime.Now.ToString("yyyy-MM-dd"));
            }

            ViewBag.UserCode = CUtils.StrValue(usercode);
            ViewBag.FullName = CUtils.StrValue(fullname);
            ViewBag.Email = CUtils.StrValue(email);
            ViewBag.CreateDTimeFrom = CUtils.StrValue(createdtimefrom);
            ViewBag.CreateDTimeTo = CUtils.StrValue(createdtimeto);
            ViewBag.BirthDayFrom = CUtils.StrValue(birthdayfrom);
            ViewBag.BirthDayTo = CUtils.StrValue(birthdayto);
            ViewBag.FlagActive = CUtils.StrValue(flagactive);

            ViewBag.StartCount = startCount;
            return View(pageInfo);
        }

        #region["Tạo mới người dùng"]
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Tạo mới người dùng";
            ViewBag.Today = Today;
            ViewBag.ListGender = ListGender;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Sys_User model)
        {
            var resultEntry = new JsonResultEntry() { Success = false };

            var createBy = "";
            model.UserName = CUtils.StrValue(model.UserCode);
            if (UserState != null && !CUtils.IsNullOrEmpty(UserState.UserName))
            {
                createBy = CUtils.StrTrim(UserState.UserName);
            }
            model.CreateBy = createBy;
            Sys_UserManager.Add(model);
            resultEntry.Success = true;
            resultEntry.AddMessage("Tạo mới người dùng thành công!");

            return Json(resultEntry);

        }

        #endregion
        #region["Thay đổi thông tin người dùng"]
        [HttpGet]
        public ActionResult Update(string usercode = "")
        {
            if (CUtils.IsNullOrEmpty(usercode))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sysUser = Sys_UserManager.Get(new Sys_User() { UserCode = usercode });

            if (sysUser != null)
            {
                return View(sysUser);
            }
            else
            {
                return HttpNotFound();
            }


            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Sys_User model)
        {
            var resultEntry = new JsonResultEntry() { Success = false };
            var exitsData = "";
            if (model != null && !CUtils.IsNullOrEmpty(model.UserCode))
            {
                var sysUser = Sys_UserManager.Get(new Sys_User() { UserCode = model.UserCode });
                if (sysUser != null)
                {
                    var updateBy = "";
                    if (UserState != null && !CUtils.IsNullOrEmpty(UserState.UserName))
                    {
                        updateBy = CUtils.StrTrim(UserState.UserName);
                    }
                    model.UpdateBy = updateBy;
                    model.IsSysAdmin = sysUser.IsSysAdmin; // Không update cờ SysAdmin
                    Sys_UserManager.Update(model, sysUser);
                    resultEntry.Success = true;
                    resultEntry.AddMessage("Cập nhật người dùng thành công!");
                    resultEntry.RedirectUrl = Url.Action("Index", "Sys_User", new { area = "Auth" });
                }
                else
                {
                    resultEntry.Success = true;
                    exitsData = "Mã người dùng '" + model.UserCode + "' không có trong hệ thống!";
                }
            }
            else
            {
                resultEntry.Success = true;
                exitsData = "Mã người dùng trống!";
            }
            resultEntry.AddMessage(exitsData);

            return Json(resultEntry);
        }

        #endregion
        #region["Thông tin người dùng"]
        [HttpGet]
        public ActionResult Detail(string usercode = "")
        {
            /* if (CUtils.IsNullOrEmpty(usercode))
             {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
             }
             var sysUser = Sys_UserManager.Get(new Sys_User() { UserCode = usercode });

             if (sysUser != null)
             {
                 return View(sysUser);
             }
             else
             {
                 return HttpNotFound();
             }*/
            return View();
        }
        #endregion
        #region["Xóa thông tin người dùng"]
        [HttpPost]
        public ActionResult Delete()
        {
            return null;
        }
        #endregion
        #region["Change Password"]
        [HttpPost]
        public ActionResult ShowPopupChangePassword(string usercode)
        {
            var resultEntry = new JsonResultEntry() { Success = false };
            try
            {
                ViewBag.UserCode = usercode;
                return JsonView("ShowPopupChangePassword", null);
            }
            catch (Exception ex)
            {
                resultEntry.SetFailed().AddException(this, ex);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return JsonViewError("ShowPopupChangePassword", null, resultEntry);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(string usercode, string passnew, string confpass)
        {
            var resultEntry = new JsonResultEntry() { Success = false };
            var userState = this.UserState;
            try
            {
                var title = "";
                if (!CUtils.IsNullOrEmpty(usercode))
                {
                    if (!CUtils.IsNullOrEmpty(passnew))
                    {
                        passnew = passnew.Trim();
                        if (!CUtils.IsNullOrEmpty(confpass))
                        {
                            confpass = confpass.Trim();
                            if (passnew.Equals(confpass))
                            {
                                var sysUser = Sys_UserManager.Get(new Sys_User() { UserCode = usercode });

                                if (sysUser != null)
                                {
                                    var salt = EncryptUtils.GenerateSalt();
                                    var password = EncryptUtils.EncryptPassword(passnew, salt);

                                    sysUser.Password = password.Trim();
                                    sysUser.PasswordSalt = salt.Trim();
                                    sysUser.UpdateBy = userState.SysUser.UserCode;

                                    Sys_UserManager.Add(sysUser);
                                    title = "Thay đổi mật khẩu thành công!";

                                    resultEntry.RedirectUrl = Url.Action("LogOff", "Account", new { area = "Auth" });

                                }
                                else
                                {
                                    title = "Mã người dùng '" + usercode + "' không có trong hệ thống!";
                                }
                            }
                            else
                            {
                                title = "Nhập lại mật khẩu mới không đúng, Vui lòng nhập lại!";
                            }
                        }
                        else
                        {
                            title = "Nhập lại khẩu mới trống!";
                        }
                    }
                    else
                    {
                        title = "Mật khẩu mới trống!";
                    }
                }
                else
                {
                    title = "Mã người dùng trống!";
                }

                resultEntry.Success = true;
                resultEntry.AddMessage(title);


            }
            catch (Exception ex)
            {
                resultEntry.SetFailed().AddException(this, ex);
            }
            return Json(resultEntry);

        }
        #endregion

    }
}