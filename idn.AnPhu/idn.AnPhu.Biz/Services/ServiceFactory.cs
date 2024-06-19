using idn.AnPhu.Biz.Persistance.SqlServer;
using System.Collections;

namespace idn.AnPhu.Biz.Services
{
    public class ServiceFactory
    {
        static Hashtable services = new Hashtable();

        static ServiceFactory()
        {
            #region["Auth"] 
            services.Add(typeof(Sys_UserManager), new Sys_UserManager(new Sys_UserProvider()));
            services.Add(typeof(Sys_GroupManager), new Sys_GroupManager(new Sys_GroupProvider()));
            // ThangPV
            services.Add(typeof(PrdCategoriesManager), new PrdCategoriesManager(new PrdCategoriesProvider()));
            services.Add(typeof(ProductManager), new ProductManager(new ProductProvider()));
            services.Add(typeof(ProductReviewsManager), new ProductReviewsManager(new ProductReviewsProvider()));
            services.Add(typeof(ProductPropertiesManager), new ProductPropertiesManager(new ProductPropertiesProvider()));
            services.Add(typeof(ProductVersionsManager), new ProductVersionsManager(new ProductVersionsProvider()));
            #endregion
        }

        public static T GetService<T>()
        {
            foreach (var service in services.Values)
            {
                if (service is T)
                {
                    return (T)service;
                }
            }
            return default(T);
        }

        public static Sys_UserManager Sys_UserManager
        {
            get
            {
                return (Sys_UserManager)services[typeof(Sys_UserManager)];
            }
            set
            {
                services[typeof(Sys_UserManager)] = value;
            }
        }
        public static Sys_GroupManager Sys_GroupManager
        {
            get
            {
                return (Sys_GroupManager)services[typeof(Sys_GroupManager)];
            }
            set
            {
                services[typeof(Sys_GroupManager)] = value;
            }
        }

        public static PrdCategoriesManager PrdCategoriesManager
        {
            get
            {
                return (PrdCategoriesManager)services[typeof(PrdCategoriesManager)];
            }
            set
            {
                services[typeof(PrdCategoriesManager)] = value;
            }
        }

        public static ProductManager ProductManager
        {
            get
            {
                return (ProductManager)services[typeof(ProductManager)];
            }
            set
            {
                services[typeof(ProductManager)] = value;
            }
        }

        public static ProductReviewsManager ProductReviewsManager
        {
            get
            {
                return (ProductReviewsManager)services[typeof(ProductReviewsManager)];
            }
            set
            {
                services[typeof(ProductReviewsManager)] = value;
            }
        }

        public static ProductPropertiesManager ProductPropertiesManager
        {
            get
            {
                return (ProductPropertiesManager)services[typeof(ProductPropertiesManager)];
            }
            set
            {
                services[typeof(ProductPropertiesManager)] = value;
            }
        }

        public static ProductVersionsManager ProductVersionsManager
        {
            get
            {
                return (ProductVersionsManager)services[typeof(ProductVersionsManager)];
            }
            set
            {
                services[typeof(ProductVersionsManager)] = value;
            }
        }
    }
}
