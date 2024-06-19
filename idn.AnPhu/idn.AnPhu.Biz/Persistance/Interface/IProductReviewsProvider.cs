using idn.AnPhu.Biz.Models;
using System.Collections.Generic;

namespace idn.AnPhu.Biz.Persistance.Interface
{
    interface IProductReviewsProvider : IImportableDataProvider<ProductReviews>
    {
        List<ProductReviews> GetAllReview(int productId, int startIndex, int count, ref int totalItems);
        List<ProductReviews> GetAllActive(int productId);
        List<ProductReviews> Search(int productId, string txtSearch, int startIndex, int pageCount, ref int totalItems);
    }
}
