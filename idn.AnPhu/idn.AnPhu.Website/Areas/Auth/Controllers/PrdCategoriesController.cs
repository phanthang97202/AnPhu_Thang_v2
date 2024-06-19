using Client.Core.Data.Entities.Paging;
using Client.Core.Extensions;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Services;
using idn.AnPhu.Utils;
using idn.AnPhu.Website.Controllers;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using TConst = idn.AnPhu.Constants;

namespace idn.AnPhu.Website.Areas.Auth.Controllers
{
    public class PrdCategoriesController : AdministratorController
    {
        private PrdCategoriesManager PrdCategoriesManager
        {
            get { return ServiceFactory.PrdCategoriesManager; }
        }

        public ActionResult Index(int? page, int? pageSize, string txtSearch = "")
        {
            var pageInfo = new PageInfo<PrdCategories>(0, PageSizeAdminConfig);
            if (page != null && pageSize != null)
            {
                pageInfo.PageIndex = (int)page;
                pageInfo.PageSize = (int)pageSize;
            }
            else
            {
                pageInfo.PageIndex = 1;
                pageInfo.PageSize = TConst.Nonsense.MAX_PAGE_SIZE; // Hard code to get all data
            }

            var pageView = "";
            var lastRecord = 0;

            pageInfo = PrdCategoriesManager.Search(txtSearch, pageInfo.PageIndex, pageInfo.PageSize);

            if (pageInfo != null && pageInfo.DataList != null && pageInfo.DataList.Count > 0)
            {
                if ((pageInfo.PageIndex * pageInfo.PageSize) > pageInfo.ItemCount)
                {
                    lastRecord = pageInfo.ItemCount;
                }
                else
                {
                    lastRecord = pageInfo.PageIndex * pageInfo.PageSize;
                }
                pageView = "Showing " + ((pageInfo.PageIndex - 1) * pageInfo.PageSize + 1) + " to " + lastRecord + " of " + pageInfo.ItemCount + " entries";
            }

            ViewBag.pageView = pageView;
            var listPageSize = new int[3] { 5, 10, 15 };
            ViewBag.listPageSize = listPageSize;
            ViewBag.JsonDataList = JsonConvert.SerializeObject(pageInfo.DataList);

            return View(pageInfo);
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Tạo mới danh mục";
            ViewBag.Today = Today;
            ViewBag.message = "";
            ViewBag.ListPrdCategories = PrdCategoriesManager.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Create(PrdCategories model)
        {
            var createBy = "";
            if (UserState != null && !CUtils.IsNullOrEmpty(UserState.UserName))
            {
                createBy = CUtils.StrTrim(UserState.UserName);
            }
            model.CreateBy = createBy;

            try
            {
                model.PrdCategoryShortName = model.PrdCategoryTitle.ToUrlSegment(250).ToLower();
                PrdCategoriesManager.Add(model);
                ViewBag.message = "Thêm mới loại xe thành công";
                ViewBag.ListPrdCategories = PrdCategoriesManager.GetAll();
                //return View(model);
                return RedirectToAction("Index", "PrdCategories");
            }
            catch
            {
                ViewBag.message = "Đã có lỗi trong quá trình thêm mới loại xe";
                ViewBag.ListPrdCategories = PrdCategoriesManager.GetAll();
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Update(int prdCategoryId)
        {
            if (CUtils.IsNullOrEmpty(prdCategoryId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var prdCategory = PrdCategoriesManager.Get(new PrdCategories() { PrdCategoryId = prdCategoryId });

            if (prdCategory != null)
            {
                List<PrdCategories> listParentId = PrdCategoriesManager.GetAll();

                ViewBag.ListPrdCategories = new SelectList(listParentId, "PrdCategoryId", "PrdCategoryTitle");

                ViewBag.message = "";
                ViewBag.IsEdit = true;

                return View(prdCategory);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(PrdCategories objModify)
        {
            PrdCategories objBeforeModify = PrdCategoriesManager.Get(new PrdCategories() { PrdCategoryId = objModify.PrdCategoryId });
            PrdCategoriesManager.Update(objModify, objBeforeModify);
            List<PrdCategories> listParentId = PrdCategoriesManager.GetAll();

            //ViewBag.ListPrdCategories = new SelectList(listParentId, "ParentId", "PrdCategoryTitle");
            //ViewBag.ShowNotify = @"<div style='display: none' class='show-notifycation alert alert-success' role='alert'>Update successfully!</div>";
            //return View(objModify);
            return RedirectToAction("Index", "PrdCategories");
        }
    }
}