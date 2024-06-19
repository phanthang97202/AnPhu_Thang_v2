using idn.AnPhu.Biz.Models;
using System.Collections.Generic;

namespace idn.AnPhu.Biz.Persistance.Interface
{
    interface IProductPropertiesProvider : IImportableDataProvider<ProductProperties>
    {
        List<ProductProperties> GetAllProperties(int productId, int startIndex, int count, ref int totalItems);
        List<ProductProperties> GetAllActiveByProductId(int productId);
        List<ProductProperties> Search(int productId, string txtSearch, int startIndex, int pageCount, ref int totalItems);
    }
}
