using idn.AnPhu.Biz.Models;
using System.Collections.Generic;

namespace idn.AnPhu.Biz.Persistance.Interface
{
    interface IPrdCategoriesProvider : IImportableDataProvider<PrdCategories>
    {
        List<PrdCategories> Search(string txtSearch, int startIndex, int pageCount, ref int totalItems);
        List<PrdCategories> GetAllActive();
        PrdCategories GetByShortName(string shortName);
    }
}
