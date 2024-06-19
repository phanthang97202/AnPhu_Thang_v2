using idn.AnPhu.Biz.Models;
using idn.AnPhu.Website.Controllers;
using idn.AnPhu.Website.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace idn.AnPhu.Website.Areas.Auth.Controllers
{
    public class HomeController : AdministratorController
    {
        // GET: Auth/Home
        [CustomAuthorizeAttribute(UserRole.SysAdmin)]
        public ActionResult Index()
        {
            return View();
        }
    }
}