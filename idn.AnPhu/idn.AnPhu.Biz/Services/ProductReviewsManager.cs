using Client.Core.Data;
using Client.Core.Data.Entities.Paging;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using System.Collections.Generic;

namespace idn.AnPhu.Biz.Services
{
    public class ProductReviewsManager : DataManagerBase<ProductReviews>
    {
        public ProductReviewsManager(IDataProvider<ProductReviews> provider) : base(provider)
        {

        }

        IProductReviewsProvider ProductReviewsProvider
        {
            get
            {
                return (IProductReviewsProvider)Provider;
            }
        }

        public List<ProductReviews> GetAll(int productId)
        {
            int total = 0;
            List<ProductReviews> data = ProductReviewsProvider.GetAllReview(productId, 0, 0, ref total);
            return data;
        }

        public List<ProductReviews> GetAllActive(int productId)
        {
            List<ProductReviews> reviews = ProductReviewsProvider.GetAllActive(productId);
            return reviews;
        }

        public PageInfo<ProductReviews> Search(int productId, string txtSearch, int pageIndex, int pageSize)
        {
            int totalItems = 0;
            PageInfo<ProductReviews> pageInfo = new PageInfo<ProductReviews>(pageIndex, pageSize);
            int startIndex = (pageIndex - 1) * pageSize;
            pageInfo.DataList = ProductReviewsProvider.Search(productId, txtSearch, startIndex, pageSize, ref totalItems);
            pageInfo.ItemCount = totalItems;
            return pageInfo;
        }
    }
}
