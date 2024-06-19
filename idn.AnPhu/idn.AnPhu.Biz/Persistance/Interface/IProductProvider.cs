using idn.AnPhu.Biz.Models;
using System.Collections.Generic;

namespace idn.AnPhu.Biz.Persistance.Interface
{
    interface IProductProvider : IImportableDataProvider<Product>
    {
        List<Product> Search(string txtSearch, int startIndex, int pageCount, ref int totalItems);
        List<Product> GetByCateId(int id);
        Product GetByCode(string productCode);
    }
}
