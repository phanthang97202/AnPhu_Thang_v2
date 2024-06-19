using Client.Core.Data.Entities.Paging;
using Client.Core.Extensions;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Services;
using idn.AnPhu.Utils;
using idn.AnPhu.Website.Controllers;
using System;
using System.Web;
using System.Web.Mvc;

namespace idn.AnPhu.Website.Areas.Auth.Controllers
{
    public class ProductVersionsController : AdministratorController
    {
        // GET: Auth/ProductVersions
        public ActionResult Index(int productId, int? page, int? pageSize, string txtSearch = "")
        {
            PageInfo<ProductVersions> pageInfo = new PageInfo<ProductVersions>(0, PageSizeAdminConfig);
            if (page != null && pageSize != null)
            {
                pageInfo.PageIndex = (int)page;
                pageInfo.PageSize = (int)pageSize;
            }
            else
            {
                pageInfo.PageIndex = 1;
                pageInfo.PageSize = 10;
            }

            string pageView = "";
            int lastRecord = 0;

            pageInfo = ServiceFactory.ProductVersionsManager.Search(productId, txtSearch, pageInfo.PageIndex, pageInfo.PageSize);

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
            var listPageSize = new int[3] { 10, 15, 20 };
            ViewBag.ProductId = productId;
            ViewBag.listPageSize = listPageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.message = "";
            return View(pageInfo);
        }

        [HttpGet]
        public ActionResult Create(int productId)
        {
            ViewBag.Today = Today;
            ViewBag.ProductId = productId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductVersions model)
        {
            string createBy = "";
            if (UserState != null && !CUtils.IsNullOrEmpty(UserState.UserName))
            {
                createBy = CUtils.StrTrim(UserState.UserName);
                model.CreateBy = createBy;
                model.CreateDate = DateTime.Now;
                string title = model.VersionTitle;
                model.VersionCode = title.Trim().ToUrlSegment(250).ToLower();
            }
            try
            {
                ServiceFactory.ProductVersionsManager.Add(model);
                ViewBag.message = "Thêm mới phiên bản thành công";
                ViewBag.ProductId = model.ProductId;
                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.message = "Đã xảy ra lỗi khi thêm mới phiên bản";
                ViewBag.ProductId = model.ProductId;
                throw new HttpException(404, "Failed: " + e.Message);
            }
        }

        [HttpGet]
        public ActionResult Update(int productId, int versionId)
        {
            ViewBag.Today = Today;
            ProductVersions versionsId = new ProductVersions { ProductId = productId, VersionId = versionId };
            ProductVersions versions = ServiceFactory.ProductVersionsManager.Get(versionsId);
            ViewBag.ProductId = productId;
            ViewBag.VersionId = versionId;
            return View(versions);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ProductVersions model)
        {
            string createBy = "";
            if (UserState != null && !CUtils.IsNullOrEmpty(UserState.UserName))
            {
                createBy = CUtils.StrTrim(UserState.UserName);
                model.CreateBy = createBy;
                model.CreateDate = DateTime.Now;
                string title = model.VersionTitle;
                model.VersionCode = title.Trim().ToUrlSegment(250).ToLower();
            }
            try
            {
                ProductVersions oldObjVersions = ServiceFactory.ProductVersionsManager.Get(new ProductVersions() { VersionId = model.VersionId });
                ServiceFactory.ProductVersionsManager.Update(model, oldObjVersions);
                ViewBag.message = "Thêm mới phiên bản thành công";
                ViewBag.ProductId = model.ProductId;
                ViewBag.VersionId = model.VersionId;
                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.message = "Đã xảy ra lỗi khi thêm mới phiên bản";
                ViewBag.ProductId = model.ProductId;
                ViewBag.VersionId = model.VersionId;
                throw new HttpException(404, "Failed: " + e.Message);
            }
        }

        [HttpGet]
        public ActionResult Delete(int versionId, int productId)
        {
            ProductVersions versionsRemove = new ProductVersions { VersionId = versionId, ProductId = productId };
            ServiceFactory.ProductVersionsManager.Remove(versionsRemove);
            ViewBag.message = "Xóa phiên bản thành công";
            return RedirectToAction("Index", new { productId = productId });
        }
    }
}