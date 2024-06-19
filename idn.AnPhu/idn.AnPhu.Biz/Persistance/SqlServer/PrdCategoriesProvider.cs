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
    public class PrdCategoriesProvider : DataAccessBase, IPrdCategoriesProvider
    {
        public void Add(PrdCategories item)
        {
            DbCommand comm = this.GetCommand("Sp_PrdCategories_Create");
            comm.AddParameter<int>(this.Factory, "ParentId", item.ParentId);
            comm.AddParameter<string>(this.Factory, "PrdCategoryTitle", (item.PrdCategoryTitle != null && item.PrdCategoryTitle.Trim().Length > 0) ? item.PrdCategoryTitle.Trim() : null);
            comm.AddParameter<string>(this.Factory, "PrdCategoryShortName", (item.PrdCategoryShortName != null && item.PrdCategoryShortName.Trim().Length > 0) ? item.PrdCategoryShortName.Trim() : null);
            comm.AddParameter<string>(this.Factory, "PrdCategorySummary", (item.PrdCategorySummary != null && item.PrdCategorySummary.Trim().Length > 0) ? item.PrdCategorySummary.Trim() : null);
            comm.AddParameter<string>(this.Factory, "PrdCategoryDescription", (item.PrdCategoryDescription != null && item.PrdCategoryDescription.Trim().Length > 0) ? item.PrdCategoryDescription.Trim() : null);
            comm.AddParameter<string>(this.Factory, "PrdCategoryKeyword", (item.PrdCategoryKeyword != null && item.PrdCategoryKeyword.Trim().Length > 0) ? item.PrdCategoryKeyword.Trim() : null);
            comm.AddParameter<string>(this.Factory, "CreateBy", item.CreateBy);
            comm.AddParameter<bool>(this.Factory, "IsActive", item.IsActive);
            comm.AddParameter<int>(this.Factory, "OrderNo", item.OrderNo);
            comm.AddParameter<string>(this.Factory, "Culture", (item.Culture != null && item.Culture.Trim().Length > 0) ? item.Culture.Trim() : null);

            this.SafeExecuteNonQuery(comm);
        }

        public PrdCategories Get(PrdCategories dummy)
        {
            DbCommand comm = this.GetCommand("Sp_PrdCategories_GetById");

            comm.AddParameter<int>(this.Factory, "prdCategoryId", dummy.PrdCategoryId);

            var table = this.GetTable(comm);
            table.TableName = TableName.PrdCategories;
            return EntityBase.ParseListFromTable<PrdCategories>(table).FirstOrDefault();
        }

        public List<PrdCategories> GetAll(int startIndex, int count, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_PrdCategories_GetAll");

            DataTable table = this.GetTable(comm);

            table.TableName = TableName.PrdCategories;

            return EntityBase.ParseListFromTable<PrdCategories>(table);
        }

        public List<PrdCategories> GetAllActive()
        {
            throw new System.NotImplementedException();
        }

        public PrdCategories GetByShortName(string shortName)
        {
            throw new System.NotImplementedException();
        }

        public void Import(List<PrdCategories> list, bool deleteExist)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(PrdCategories item)
        {
            throw new System.NotImplementedException();
        }

        public List<PrdCategories> Search(string txtSearch, int startIndex, int pageSize, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_PrdCategories_Search");
            comm.AddParameter<string>(this.Factory, "txtSearch", (txtSearch != null && txtSearch.Trim().Length > 0) ? txtSearch : null);
            comm.AddParameter<int>(this.Factory, "startIndex", startIndex);
            comm.AddParameter<int>(this.Factory, "count", pageSize);

            // đại diện cho DbParameter, để sử dụng như 1 biến bình thường 
            DbParameter totalItemsParam = comm.AddParameter(this.Factory, "totalItems", DbType.Int32, null);
            totalItemsParam.Direction = ParameterDirection.Output;

            var table = this.GetTable(comm);
            table.TableName = TableName.PrdCategories;

            if (totalItemsParam.Value != DBNull.Value)
            {
                totalItems = Convert.ToInt32(totalItemsParam.Value);
            }

            return EntityBase.ParseListFromTable<PrdCategories>(table);

        }

        public void Update(PrdCategories @new, PrdCategories old)
        {
            PrdCategories objParam = @new;

            objParam.PrdCategoryId = old.PrdCategoryId;

            DbCommand comm = this.GetCommand("Sp_PrdCategories_Update");

            if (comm == null) { return; }

            comm.AddParameter<int>(this.Factory, "PrdCategoryId", objParam.PrdCategoryId);
            comm.AddParameter<int>(this.Factory, "ParentId", objParam.ParentId);
            comm.AddParameter<string>(this.Factory, "PrdCategoryTitle", (objParam.PrdCategoryTitle != null && objParam.PrdCategoryTitle.Trim().Length > 0) ? objParam.PrdCategoryTitle.Trim() : null);
            comm.AddParameter<string>(this.Factory, "PrdCategoryShortName", (objParam.PrdCategoryShortName != null && objParam.PrdCategoryShortName.Trim().Length > 0) ? objParam.PrdCategoryShortName.Trim() : null);
            comm.AddParameter<string>(this.Factory, "PrdCategorySummary", (objParam.PrdCategorySummary != null && objParam.PrdCategorySummary.Trim().Length > 0) ? objParam.PrdCategorySummary.Trim() : null);
            comm.AddParameter<string>(this.Factory, "PrdCategoryDescription", (objParam.PrdCategoryDescription != null && objParam.PrdCategoryDescription.Trim().Length > 0) ? objParam.PrdCategoryDescription.Trim() : null);
            comm.AddParameter<string>(this.Factory, "PrdCategoryKeyword", (objParam.PrdCategoryKeyword != null && objParam.PrdCategoryKeyword.Trim().Length > 0) ? objParam.PrdCategoryKeyword.Trim() : null);
            comm.AddParameter<bool>(this.Factory, "IsActive", objParam.IsActive);
            comm.AddParameter<int>(this.Factory, "OrderNo", objParam.OrderNo);
            comm.AddParameter<string>(this.Factory, "Culture", (objParam.Culture != null && objParam.Culture.Trim().Length > 0) ? objParam.Culture.Trim() : null);

            this.SafeExecuteNonQuery(comm);
        }
    }
}
