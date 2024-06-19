using Client.Core.Data.DataAccess;
using Client.Core.Data.Entities;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using idn.AnPhu.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace idn.AnPhu.Biz.Persistance.SqlServer
{
    public class ProductPropertiesProvider : DataAccessBase, IProductPropertiesProvider
    {
        public void Add(ProductProperties item)
        {
            DbCommand comm = this.GetCommand("Sp_ProductProperties_Create");

            comm.AddParameter<int>(this.Factory, "productId", item.ProductId);
            comm.AddParameter<string>(this.Factory, "productPropertyTitle", (item.ProductPropertyTitle != null && item.ProductPropertyTitle.Trim().Length > 0) ? item.ProductPropertyTitle.Trim() : null);
            comm.AddParameter<string>(this.Factory, "productPropertyBody", (item.ProductPropertyBody != null && item.ProductPropertyBody.Trim().Length > 0) ? item.ProductPropertyBody.Trim() : null);
            comm.AddParameter<bool>(this.Factory, "IsActive", item.IsActive);
            comm.AddParameter<int>(this.Factory, "OrderNo", item.OrderNo);
            comm.AddParameter<string>(this.Factory, "Culture", (item.Culture != null && item.Culture.Trim().Length > 0) ? item.Culture.Trim() : null);

            this.SafeExecuteNonQuery(comm);
        }

        public ProductProperties Get(ProductProperties dummy)
        {
            DbCommand comm = this.GetCommand("Sp_ProductProperties_GetById");

            comm.AddParameter<int>(this.Factory, "productId", dummy.ProductId);
            comm.AddParameter<int>(this.Factory, "propertyId", dummy.ProductPropertyId);

            DataTable table = this.GetTable(comm);
            table.TableName = TableName.ProductProperties;
            ProductProperties properties = EntityBase.ParseListFromTable<ProductProperties>(table).FirstOrDefault();
            return properties;
        }

        public List<ProductProperties> GetAll(int startIndex, int count, ref int totalItems)
        {
            throw new System.NotImplementedException();
        }

        public List<ProductProperties> GetAllActiveByProductId(int productId)
        {
            DbCommand comm = this.GetCommand("Sp_ProductProperties_GetAllActiveByProductId");
            comm.AddParameter<int>(this.Factory, "productId", productId);
            DataTable table = this.GetTable(comm);
            table.TableName = TableName.ProductProperties;
            List<ProductProperties> properties = EntityBase.ParseListFromTable<ProductProperties>(table);
            return properties;
        }

        public List<ProductProperties> GetAllProperties(int productId, int startIndex, int count, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_ProductProperties_GetAll");
            comm.AddParameter<int>(this.Factory, "productId", productId);
            DataTable table = this.GetTable(comm);
            table.TableName = TableName.ProductProperties;
            List<ProductProperties> properties = EntityBase.ParseListFromTable<ProductProperties>(table);
            return properties;
        }

        public void Import(List<ProductProperties> list, bool deleteExist)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(ProductProperties item)
        {
            // diff base and this and DataAccessBase
            var comm = base.GetCommand("Sp_ProductProperties_Delete");
            if (comm == null) return;
            comm.AddParameter<int>(this.Factory, "propertiesId", item.ProductPropertyId);
            this.SafeExecuteNonQuery(comm);
        }

        public List<ProductProperties> Search(int productId, string txtSearch, int startIndex, int pageSize, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_ProductProperties_Search");
            comm.AddParameter<int>(this.Factory, "productId", productId);
            comm.AddParameter<string>(this.Factory, "txtSearch", (txtSearch != null && txtSearch.Trim().Length > 0) ? txtSearch : null);
            comm.AddParameter<int>(this.Factory, "startIndex", startIndex);
            comm.AddParameter<int>(this.Factory, "count", pageSize);

            DbParameter totalItemsParam = comm.AddParameter(this.Factory, "totalItems", DbType.Int32, null);
            totalItemsParam.Direction = ParameterDirection.Output;

            DataTable table = this.GetTable(comm);
            table.TableName = TableName.ProductProperties;

            if (totalItemsParam.Value != DBNull.Value)
            {
                totalItems = Convert.ToInt32(totalItemsParam.Value);
            }
            List<ProductProperties> properties = EntityBase.ParseListFromTable<ProductProperties>(table);
            return properties;
        }

        public void Update(ProductProperties @new, ProductProperties old)
        {
            var item = @new;
            item.ProductPropertyId = old.ProductPropertyId;
            var comm = this.GetCommand("Sp_ProductProperties_Update");
            if (comm == null) return;
            comm.AddParameter<int>(this.Factory, "productId", item.ProductId);
            comm.AddParameter<int>(this.Factory, "productPropertyId", item.ProductPropertyId);
            comm.AddParameter<string>(this.Factory, "productPropertyTitle", (item.ProductPropertyTitle != null && item.ProductPropertyTitle.Trim().Length > 0) ? item.ProductPropertyTitle.Trim() : null);
            comm.AddParameter<string>(this.Factory, "productPropertyBody", (item.ProductPropertyBody != null && item.ProductPropertyBody.Trim().Length > 0) ? item.ProductPropertyBody.Trim() : null);
            comm.AddParameter<bool>(this.Factory, "IsActive", item.IsActive);
            comm.AddParameter<int>(this.Factory, "OrderNo", item.OrderNo);
            comm.AddParameter<string>(this.Factory, "Culture", (item.Culture != null && item.Culture.Trim().Length > 0) ? item.Culture.Trim() : null);

            this.SafeExecuteNonQuery(comm);
        }
    }
}
