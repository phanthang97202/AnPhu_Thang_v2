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
    public class ProductReviewsController : AdministratorController
    {
        // GET: Auth/ProductReviews
        public ActionResult Index(int productId, int? page, int? pageSize, string txtSearch = "")
        {
            PageInfo<ProductReviews> pageInfo = new PageInfo<ProductReviews>(0, PageSizeAdminConfig);
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

            pageInfo = ServiceFactory.ProductReviewsManager.Search(productId, txtSearch, pageInfo.PageIndex, pageInfo.PageSize);

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
            int[] listPageSize = new int[3] { 10, 15, 20 };
            ViewBag.listPageSize = listPageSize;
            ViewBag.txtSearch = txtSearch;
            ViewBag.message = "";
            ViewBag.ProductId = productId;
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
        public ActionResult Create(ProductReviews model)
        {
            string createBy = "";
            if (UserState != null && !CUtils.IsNullOrEmpty(UserState.UserName))
            {
                createBy = CUtils.StrTrim(UserState.UserName);
                model.CreateBy = createBy;
            }
            try
            {
                ServiceFactory.ProductReviewsManager.Add(model);
                ViewBag.message = "Thêm mới bài đánh giá thành công";
                ViewBag.ProductId = model.ProductId;
                return View(model);
            }
            catch (Exception e)
            {
                ViewBag.message = "Đã xảy ra lỗi khi thêm mới bài đánh giá";
                ViewBag.ProductId = model.ProductId;
                throw new HttpException(404, "Failed: " + e.Message);
            }
        }

        [HttpGet]
        public ActionResult Update(int productId, int reviewId)
        {
            if (CUtils.IsNullOrEmpty(reviewId) || CUtils.IsNullOrEmpty(productId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var review = ServiceFactory.ProductReviewsManager.Get(new ProductReviews() { ReviewId = reviewId, ProductId = productId });

            if (review != null)
            {
                ViewBag.message = "";
                ViewBag.IsEdit = true;
                ViewBag.ProductId = review.ProductId;
                return View(review);
            }
            else
            {
                return HttpNotFound();
            }
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Update(ProductReviews model)
        {
            string message = "";
            if (model != null && !CUtils.IsNullOrEmpty(model.ReviewId))
            {
                var review = ServiceFactory.ProductReviewsManager.Get(new ProductReviews() { ReviewId = model.ReviewId, ProductId = model.ProductId });
                if (review != null)
                {
                    ServiceFactory.ProductReviewsManager.Update(model, review);
                    message = "Cập nhật bài viết đánh giá thành công!";
                }
                else
                {
                    message = "Mã bài viết đánh giá '" + model.ReviewId + "' không có trong hệ thống!";
                }
            }
            else
            {
                message = "Mã bài viết đánh giá trống!";
            }
            ViewBag.message = message;
            ViewBag.IsEdit = true;
            ViewBag.ProductId = model.ProductId;
            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int reviewId, int productId)
        {
            if (CUtils.IsNullOrEmpty(reviewId))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                ServiceFactory.ProductReviewsManager.Remove(new ProductReviews() { ReviewId = reviewId });
                ViewBag.message = "Xóa bài đánh giá mã " + reviewId + "thành công";
                return RedirectToAction("Index", new { productId = productId });
            }
            catch (Exception e)
            {
                ViewBag.message = "Xóa bài đánh giá" + reviewId + "thất bại";
                return RedirectToAction("Index", new { productId = productId });
            }
        }
    }
}