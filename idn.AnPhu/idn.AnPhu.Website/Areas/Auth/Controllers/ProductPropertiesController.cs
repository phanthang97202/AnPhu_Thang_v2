using Client.Core.Data.Entities.Paging;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Services;
using idn.AnPhu.Utils;
using idn.AnPhu.Website.Controllers;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace idn.AnPhu.Website.Areas.Auth.Controllers
{
    public class ProductPropertiesController : AdministratorController
    {
        // GET: Auth/Properties
        public ActionResult Index(int productId, int? page, int? pageSize, string txtSearch = "")
        {
            PageInfo<ProductProperties> pageInfo = new PageInfo<ProductProperties>(0, PageSizeAdminConfig);
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

            pageInfo = ServiceFactory.ProductPropertiesManager.Search(productId, txtSearch, pageInfo.PageIndex, pageInfo.PageSize);

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

            Product product = ServiceFactory.ProductManager.Get(new Product() { ProductId = productId });
            ViewBag.ProductName = product.ProductName;
            ViewBag.ProductId = productId;
            ViewBag.pageView = pageView;
            int[] listPageSize = new int[3] { 10, 15, 20 };
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

        [HttpPost, ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ProductProperties model)
        {
            try
            {
                ServiceFactory.ProductPropertiesManager.Add(model);
                ViewBag.message = "Thêm mới thuộc tính thành công";
                ViewBag.ProductId = model.ProductId;
                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.message = "Đã xảy ra lỗi khi thêm mới thuộc tính";
                ViewBag.ProductId = model.ProductId;
                throw new HttpException(404, "Failed: " + e.Message);
            }
        }

        [HttpGet]
        public ActionResult Update(int productId, int productPropertyId)
        {
            if (CUtils.IsNullOrEmpty(productPropertyId) || CUtils.IsNullOrEmpty(productId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProperties property = ServiceFactory.ProductPropertiesManager.Get(new ProductProperties() { ProductPropertyId = productPropertyId, ProductId = productId });

            if (property != null)
            {
                ViewBag.message = "";
                ViewBag.IsEdit = true;
                ViewBag.ProductId = productId;
                ViewBag.Check = property.IsActive ? "checked" : "";
                return View(property);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ProductProperties model)
        {
            string message = "";
            if (model != null && !CUtils.IsNullOrEmpty(model.ProductPropertyId))
            {
                ProductProperties property = ServiceFactory.ProductPropertiesManager.Get(new ProductProperties() { ProductPropertyId = model.ProductPropertyId, ProductId = model.ProductId });
                if (property != null)
                {
                    ServiceFactory.ProductPropertiesManager.Update(model, property);
                    message = "Cập nhật thông tin thuộc tính thành công!";
                }
                else
                {
                    message = "Mã thuộc tính '" + model.ProductPropertyId + "' không có trong hệ thống!";
                }
            }
            else
            {
                message = "Mã thuộc tính xe trống!";
            }
            ViewBag.message = message;
            ViewBag.ProductId = model.ProductId;
            ViewBag.IsEdit = true;
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int propertyId, int productId)
        {
            if (CUtils.IsNullOrEmpty(propertyId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                ServiceFactory.ProductPropertiesManager.Remove(new ProductProperties() { ProductPropertyId = propertyId });
                ViewBag.message = "Xóa thuộc tính mã " + propertyId + "thành công";
                return RedirectToAction("Index", new { productId = productId });
            }
            catch (Exception e)
            {
                ViewBag.message = "Xóa thuộc tính mã " + propertyId + "thất bại";
                return RedirectToAction("Index", new { productId = productId });
            }
        }

    }
}