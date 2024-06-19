using idn.AnPhu.Utils;
using idn.AnPhu.Website.Filters;
using idn.AnPhu.Website.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace idn.AnPhu.Website.Controllers
{
    [CustomErrorHandler]
    public class BaseController : Controller
    {
        public DateTime DateTimeNow()
        {
            return DateTime.Now;
        }

        public string Now
        {
            get { return DateTimeNow().ToString("yyyy-MM-dd HH:mm:ss"); }
        }
        public string Today
        {
            get { return DateTimeNow().ToString("yyyy-MM-dd"); }
        }

        public string GetRandomId(string pre, int length = 8)
        {
            string ran = Guid.NewGuid().ToString("N").Substring(0, length).ToUpper();
            return string.Format("{0}{1}", pre, ran);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public string RenderRazorViewToString(string viewName, string masterName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(ControllerContext,
                                                                         viewName, masterName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        public ActionResult JsonView(string viewname, string masterName, object model)
        {
            if (CUtils.IsNullOrEmpty(viewname))
                viewname = (string)ControllerContext.RouteData.Values["action"];
            string html = "";

            try
            {
                html = RenderRazorViewToString(viewname, masterName, model);
                var data = new
                {
                    Success = true,
                    Html = html
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var data = new
                {
                    Success = false,
                    Message = e.Message,
                    Detail = e.StackTrace
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult JsonView(string viewname, object model)
        {
            if (CUtils.IsNullOrEmpty(viewname))
                viewname = (string)ControllerContext.RouteData.Values["action"];
            string html = "";

            try
            {
                html = RenderRazorViewToString(viewname, model);
                var data = new
                {
                    Success = true,
                    Html = html
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                var data = new
                {
                    Success = false,
                    Message = e.Message,
                    Detail = e.StackTrace
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult JsonViewError(string viewname, object model, JsonResultEntry resultEntry)
        {
            if (CUtils.IsNullOrEmpty(viewname))
                viewname = (string)ControllerContext.RouteData.Values["action"];
            string html = "";
            try
            {
                html = RenderRazorViewToString(viewname, model);
                if (resultEntry.Success)
                {
                    var data = new
                    {
                        Success = true,
                        Html = html,
                        Title = resultEntry.Messages
                    };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var data = new
                    {
                        Success = false,
                        Html = html,
                        Title = resultEntry.Messages,
                        Detail = resultEntry.Detail,
                    };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                var data = new
                {
                    Success = false,
                    Message = e.Message,
                    Detail = resultEntry.Detail
                };
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult JsonView(object model = null)
        {
            return JsonView("", model);
        }

        #region["Common"]
        public class Gender
        {
            public string GenderCode { get; set; }
            public string GenderName { get; set; }
        }

        public static List<Gender> ListGender = new List<Gender>() {
            new Gender(){ GenderCode = "MALE", GenderName = "Nam"},
            new Gender(){ GenderCode = "FEMALE", GenderName = "Nữ"},
            new Gender(){ GenderCode = "OTHER", GenderName = "Khác"},
        };

        public static string GetGenderName(string gendercode)
        {
            var gendername = "";
            switch (gendercode)
            {
                case "MALE":
                    gendername = "Nam";
                    break;
                case "FEMALE":
                    gendername = "Nữ";
                    break;
                case "OTHER":
                    gendername = "Khác";
                    break;
                default:
                    gendername = "";
                    break;
            }
            return gendername;
        }
        #endregion
    }
}