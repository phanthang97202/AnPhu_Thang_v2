using Client.Core.Data;
using Client.Core.Data.Entities.Paging;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using System.Collections.Generic;

namespace idn.AnPhu.Biz.Services
{
    public class ProductManager : DataManagerBase<Product>
    {
        public ProductManager(IDataProvider<Product> provider) : base(provider)
        {

        }

        IProductProvider ProductProvider
        {
            get
            {
                return (IProductProvider)Provider;
            }
        }

        public List<Product> GetAll()
        {
            int total = 0;
            return ProductProvider.GetAll(0, 0, ref total);
        }

        public Product GetByCode(string productCode)
        {
            return ProductProvider.GetByCode(productCode);
        }

        public List<Product> GetByCateId(int id)
        {
            return ProductProvider.GetByCateId(id);
        }

        public override void Add(Product item)
        {
            base.Add(item);
        }

        public PageInfo<Product> Search(string txtSearch, int pageIndex, int pageSize)
        {
            int totalItems = 0;
            var pageInfo = new PageInfo<Product>(pageIndex, pageSize);
            var startIndex = (pageIndex - 1) * pageSize;
            pageInfo.DataList = ProductProvider.Search(txtSearch, startIndex, pageSize, ref totalItems);
            pageInfo.ItemCount = totalItems;
            return pageInfo;
        }
    }
}
