using Client.Core.Data;
using Client.Core.Data.Entities.Paging;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using System.Collections.Generic;

namespace idn.AnPhu.Biz.Services
{
    public class ProductPropertiesManager : DataManagerBase<ProductProperties>
    {
        public ProductPropertiesManager(IDataProvider<ProductProperties> provider) : base(provider)
        {

        }

        IProductPropertiesProvider ProductPropertiesProvider
        {

            get
            {
                return (IProductPropertiesProvider)Provider;
            }
        }

        public List<ProductProperties> GetAll(int productId)
        {
            int total = 0;
            List<ProductProperties> properties = ProductPropertiesProvider.GetAllProperties(productId, 0, 0, ref total);
            return properties;
        }

        public List<ProductProperties> GetAllActiveByProductId(int productId)
        {
            List<ProductProperties> properties = ProductPropertiesProvider.GetAllActiveByProductId(productId);
            return properties;
        }

        public PageInfo<ProductProperties> Search(int productId, string txtSearch, int pageIndex, int pageSize)
        {
            int totalItems = 0;
            PageInfo<ProductProperties> pageInfo = new PageInfo<ProductProperties>(pageIndex, pageSize);
            int startIndex = (pageIndex - 1) * pageSize;
            pageInfo.DataList = ProductPropertiesProvider.Search(productId, txtSearch, startIndex, pageSize, ref totalItems);
            pageInfo.ItemCount = totalItems;
            return pageInfo;
        }
    }
}
