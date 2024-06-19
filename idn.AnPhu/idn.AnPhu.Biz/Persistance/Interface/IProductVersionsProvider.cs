using idn.AnPhu.Biz.Models;
using System.Collections.Generic;

namespace idn.AnPhu.Biz.Persistance.Interface
{
    interface IProductVersionsProvider : IImportableDataProvider<ProductVersions>
    {
        List<ProductVersions> GetAllVersions(int productId, int startIndex, int count, ref int totalItems);
        List<ProductVersions> Search(int productId, string txtSearch, int startIndex, int pageCount, ref int totalItems);
    }
}
