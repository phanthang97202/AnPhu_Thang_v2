using Client.Core.Data;
using Client.Core.Data.Entities.Paging;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using System.Collections.Generic;

namespace idn.AnPhu.Biz.Services
{
    public class PrdCategoriesManager : DataManagerBase<PrdCategories>
    {
        public PrdCategoriesManager(IDataProvider<PrdCategories> provider) : base(provider)
        {

        }

        IPrdCategoriesProvider PrdCategoriesProvider
        {
            get
            {
                return (IPrdCategoriesProvider)Provider;
            }
        }

        public PageInfo<PrdCategories> Search(string txtSearch, int pageIndex, int pageSize)
        {
            int totalItems = 0;
            var pageInfo = new PageInfo<PrdCategories>(pageIndex, pageSize);
            var startIndex = (pageIndex - 1) * pageSize;

            pageInfo.DataList = PrdCategoriesProvider.Search(txtSearch, startIndex, pageSize, ref totalItems);
            pageInfo.ItemCount = totalItems;

            return pageInfo;
        }

        public List<PrdCategories> GetAll()
        {
            int total = 0;
            return PrdCategoriesProvider.GetAll(0, 0, ref total);
        }

        public List<PrdCategories> Get()
        {
            int total = 0;
            return PrdCategoriesProvider.GetAll(0, 0, ref total);
        }
    }
}
