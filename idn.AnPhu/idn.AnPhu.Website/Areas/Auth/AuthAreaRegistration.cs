using System.Web.Mvc;

namespace idn.AnPhu.Website.Areas.Auth
{
    public class AuthAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Auth";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
                "auth_login",
                "{culture}/dang-nhap",
                new { culture = "vi", controller = "Account", action = "Login" },
                new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
            );

            context.MapRoute(
                "auth_home",
                "{culture}/quan-tri",
                new { culture = "vi", controller = "Home", action = "Index" },
                new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
            );

            context.MapRoute(
                "auth_sys_user",
                "{culture}/quan-tri/nguoi-dung",
                new { culture = "vi", controller = "Sys_User", action = "Index" },
                new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
            );
            context.MapRoute(
                name: "auth_sys_user_create",
                url: "{culture}/quan-tri/nguoi-dung/tao-moi",
                defaults: new
                {
                    culture = "vi",
                    area = "Auth",
                    controller = "Sys_User",
                    action = "Create",
                },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
            );

            context.MapRoute(
               name: "auth_sys_user_update",
               url: "{culture}/quan-tri/nguoi-dung/cap-nhat",
               defaults: new
               {
                   culture = "vi",
                   area = "Auth",
                   controller = "Sys_User",
                   action = "Update",
               },
               namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
            );
            context.MapRoute(
              name: "auth_sys_user_detail",
              url: "{culture}/quan-tri/nguoi-dung/chi-tiet",
              defaults: new
              {
                  culture = "vi",
                  area = "Auth",
                  controller = "Sys_User",
                  action = "Detail",
              },
              namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
            );
            context.MapRoute(
                "Auth_default",
                "{culture}/Auth/{controller}/{action}/{id}",
                new { culture = "vi", controller = "Home", action = "Index", id = UrlParameter.Optional },
                new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
            );
            //context.MapRoute(
            //    "Auth_default",
            //    "Auth/{controller}/{action}/{id}",
            //    new { action = "Index", id = UrlParameter.Optional }
            //);

            // PHANTHANG START ROUTER
            #region //// Tab danh mục sản phẩm
            context.MapRoute(
                "auth_prdcategories",
                url: "{culture}/quan-tri/danh-muc-san-pham/danh-sach",
                defaults: new { culture = "vi", area = "Auth", controller = "PrdCategories", action = "Index" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_prdcategories_create",
                url: "{culture}/quan-tri/danh-muc-san-pham/tao-moi",
                defaults: new { culture = "vi", area = "Auth", controller = "PrdCategories", action = "Create" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_prdcategories_update",
                url: "{culture}/quan-tri/danh-muc-san-pham/cap-nhat/{prdCategoryId}",
                defaults: new { culture = "vi", area = "Auth", controller = "PrdCategories", action = "Update", prdCategoryId = UrlParameter.Optional },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );
            #endregion

            #region //// Tab product 
            context.MapRoute(
                "auth_product",
                url: "{culture}/quan-tri/danh-sach-san-pham/danh-sach",
                defaults: new { culture = "vi", area = "Auth", controller = "Product", action = "Index" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_product_create",
                url: "{culture}/quan-tri/danh-sach-san-pham/tao-moi",
                defaults: new { culture = "vi", area = "Auth", controller = "Product", action = "Create" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_product_update",
                url: "{culture}/quan-tri/danh-sach-san-pham/cap-nhat",
                defaults: new { culture = "vi", area = "Auth", controller = "Product", action = "Update" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );
            #endregion

            #region //// Tab versions
            context.MapRoute(
                "auth_productversions",
                url: "{culture}/quan-tri/danh-sach-version/danh-sach",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductVersions", action = "Index" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_productversions_create",
                url: "{culture}/quan-tri/danh-sach-version/tao-moi",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductVersions", action = "Create" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_productversions_update",
                url: "{culture}/quan-tri/danh-sach-version/cap-nhat",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductVersions", action = "Update" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_productversions_delete",
                url: "{culture}/quan-tri/danh-sach-version/xoa",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductVersions", action = "Delete" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );
            #endregion

            #region //// Tab properties
            context.MapRoute(
                "auth_productproperties",
                url: "{culture}/quan-tri/danh-sach-thuoc-tinh/danh-sach",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductProperties", action = "Index" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_productproperties_create",
                url: "{culture}/quan-tri/danh-sach-thuoc-tinh/tao-moi",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductProperties", action = "Create" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_productproperties_update",
                url: "{culture}/quan-tri/danh-sach-thuoc-tinh/cap-nhat",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductProperties", action = "Update" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_productproperties_delete",
                url: "{culture}/quan-tri/danh-sach-thuoc-tinh/xoa",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductProperties", action = "Delete" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );
            #endregion

            #region //// Tab reviews
            context.MapRoute(
                "auth_productreviews",
                url: "{culture}/quan-tri/danh-sach-review/danh-sach",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductReviews", action = "Index" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_productreviews_create",
                url: "{culture}/quan-tri/danh-sach-review/tao-moi",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductReviews", action = "Create" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_productreviews_update",
                url: "{culture}/quan-tri/danh-sach-review/cap-nhat",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductReviews", action = "Update" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );

            context.MapRoute(
                "auth_productreviews_delete",
                url: "{culture}/quan-tri/danh-sach-review/xoa",
                defaults: new { culture = "vi", area = "Auth", controller = "ProductReviews", action = "Delete" },
                namespaces: new[] { "idn.AnPhu.Website.Areas.Auth.Controllers" }
                );
            #endregion
        }
    }
}