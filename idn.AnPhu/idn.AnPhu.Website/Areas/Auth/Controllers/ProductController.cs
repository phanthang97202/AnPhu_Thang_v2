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
    public class ProductController : AdministratorController
    {
        // GET: Auth/Product
        public ActionResult Index(int? page, int? pageSize, string txtSearch = "")
        {
            //List<Product> products = ServiceFactory.ProductManager.GetAll(); 

            PageInfo<Product> pageInfo = new PageInfo<Product>(0, PageSizeAdminConfig);
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

            string pageView = "";
            int lastRecord = 0;

            pageInfo = ServiceFactory.ProductManager.Search(txtSearch, pageInfo.PageIndex, pageInfo.PageSize);

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
            int[] listPageSize = new int[3] { 5, 10, 15 };
            ViewBag.listPageSize = listPageSize;
            ViewBag.JsonDataList = JsonConvert.SerializeObject(pageInfo.DataList);

            return View(pageInfo);
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Title = "Tạo mới sản phẩm";
            ViewBag.Today = Today;
            List<PrdCategories> listParentId = ServiceFactory.PrdCategoriesManager.GetAll();
            ViewBag.ListPrdCategories = ServiceFactory.PrdCategoriesManager.GetAll();
            //ViewBag.ListPrdCategories = new SelectList(listParentId, "PrdCategoryId", "PrdCategoryTitle");
            ViewBag.message = "";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            string createBy = "";
            if (UserState != null && !CUtils.IsNullOrEmpty(UserState.UserName))
            {
                createBy = CUtils.StrTrim(UserState.UserName);
            }
            product.CreateBy = createBy;
            product.ProductCode = product.ProductName.ToUrlSegment(250).ToLower();
            try
            {
                ServiceFactory.ProductManager.Add(product);
                ViewBag.message = "Thêm mới sản phẩm thành công";
                ViewBag.ListPrdCategories = ServiceFactory.PrdCategoriesManager.GetAll();
                return View(product);
            }
            catch
            {
                ViewBag.message = "Đã có lỗi trong quá trình thêm mới sản phẩm";
                ViewBag.ListPrdCategories = ServiceFactory.PrdCategoriesManager.GetAll();
                return View(product);
            }
        }

        [HttpGet]
        public ActionResult Update(int productId)
        {
            if (CUtils.IsNullOrEmpty(productId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = ServiceFactory.ProductManager.Get(new Product() { ProductId = productId });

            if (product != null)
            {
                ViewBag.message = "";
                ViewBag.IsEdit = true;
                ViewBag.ListPrdCategories = ServiceFactory.PrdCategoriesManager.GetAll();
                return View(product);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Product model)
        {
            string message = "";
            if (model != null && !CUtils.IsNullOrEmpty(model.ProductId))
            {
                Product product = ServiceFactory.ProductManager.Get(new Product() { ProductId = model.ProductId });
                if (product != null)
                {
                    model.ProductCode = model.ProductName.ToUrlSegment(250).ToLower();
                    ServiceFactory.ProductManager.Update(model, product);
                    message = "Cập nhật thông tin sản phẩm thành công!";
                }
                else
                {
                    message = "Mã sản phẩm '" + model.ProductId + "' không có trong hệ thống!";
                }
            }
            else
            {
                message = "Mã sản phẩm trống!";
            }
            ViewBag.message = message;
            ViewBag.IsEdit = true;
            ViewBag.ListPrdCategories = ServiceFactory.PrdCategoriesManager.GetAll();
            return View(model);


        }
    }
}