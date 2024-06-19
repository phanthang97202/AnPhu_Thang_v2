using Client.Core.Data.DataAccess;
using Client.Core.Data.Entities;
using Client.Core.Data.Entities.Paging;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using idn.AnPhu.Constants;
using idn.AnPhu.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Persistance.SqlServer
{
    public class Sys_GroupProvider : DataAccessBase, ISys_GroupProvider
    {
        public Sys_Group Get(Sys_Group dummy)
        {
            DbCommand comm = this.GetCommand("Sp_Sys_Group_GetByGroupCode");

            comm.AddParameter<string>(this.Factory, "GroupCode", dummy.GroupCode);

            var table = this.GetTable(comm);
            table.TableName = TableName.Sys_Group;
            //if (table == null || table.Rows.Count == 0)
            //{
            //    throw new SystemException("Loi nguy hiem", new Exception());
            //}
            return EntityBase.ParseListFromTable<Sys_Group>(table).FirstOrDefault();
        }

        public List<Sys_Group> GetAll(int startIndex, int count, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_Sys_Group_GetAll");

            //comm.AddParameter<int>(this.Factory, "StartIndex", startIndex);
            //comm.AddParameter<int>(this.Factory, "Count", count);

            //DbParameter totalItemsParam = comm.AddParameter(this.Factory, "totalItems", DbType.Int32, null);
            //totalItemsParam.Direction = ParameterDirection.Output;

            var table = this.GetTable(comm);
            table.TableName = TableName.Sys_User;

            //if (totalItemsParam.Value != DBNull.Value)
            //{
            //    totalItems = Convert.ToInt32(totalItemsParam.Value);
            //}
            return EntityBase.ParseListFromTable<Sys_Group>(table);
        }

        public List<Sys_Group> Search(string groupCode, string groupName, string description, string flagActive, int pageIndex, int pageCount, ref int totalItems)
        {
            DbCommand comm = this.GetCommand("Sp_Sys_Group_Search");
            comm.AddParameter<string>(this.Factory, "groupCode", (groupCode != null && groupCode.Trim().Length > 0) ? groupCode : null);
            comm.AddParameter<string>(this.Factory, "groupName", (groupName != null && groupName.Trim().Length > 0) ? groupName.Trim() : null);
            comm.AddParameter<string>(this.Factory, "description", (description != null && description.Trim().Length > 0) ? description.Trim() : null);
            comm.AddParameter<string>(this.Factory, "flagActive", flagActive);
            comm.AddParameter<int>(this.Factory, "startIndex", pageIndex);
            comm.AddParameter<int>(this.Factory, "count", pageCount);

            DbParameter totalItemsParam = comm.AddParameter(this.Factory, "totalItems", DbType.Int32, null);
            totalItemsParam.Direction = ParameterDirection.Output;

            var table = this.GetTable(comm);
            table.TableName = TableName.Sys_Group;

            if (totalItemsParam.Value != DBNull.Value)
            {
                totalItems = Convert.ToInt32(totalItemsParam.Value);
            }
            return EntityBase.ParseListFromTable<Sys_Group>(table);
        }

        public PageInfo<Sys_Group> Search(PageInfo<Sys_Group> pageInfoSearch)
        {
            var sys_Group = pageInfoSearch.DataList[0];
            var pageIndex = CUtils.ConvertToInt32(CUtils.StrTrim(pageInfoSearch.PageIndex));
            var pageSize = CUtils.ConvertToInt32(CUtils.StrTrim(pageInfoSearch.PageSize));
            var comm = this.GetCommand("Sp_Sys_Group_Search");
            comm.AddParameter<string>(this.Factory, "groupCode", CUtils.StrTrim(sys_Group.GroupCode));
            comm.AddParameter<string>(this.Factory, "groupName", CUtils.StrTrim(sys_Group.GroupName));
            comm.AddParameter<string>(this.Factory, "description", CUtils.StrTrim(sys_Group.Description));
            comm.AddParameter<string>(this.Factory, "flagActive", CUtils.StrTrim(sys_Group.FlagActive));
            comm.AddParameter<int>(this.Factory, "startIndex", pageIndex);
            comm.AddParameter<int>(this.Factory, "count", pageSize);

            var totalItemsParam = comm.AddParameter(this.Factory, "totalItems", DbType.Int32, null);
            totalItemsParam.Direction = ParameterDirection.Output;

            var table = this.GetTable(comm);
            table.TableName = TableName.Sys_Group;

            var itemCount = CUtils.ConvertToInt32(CUtils.StrTrim(totalItemsParam.Value));
            var pageCount = (itemCount % pageSize == 0) ? itemCount / pageSize : itemCount / pageSize + 1;

            var pageInfo = new PageInfo<Sys_Group>(0, 0) { DataList = new List<Sys_Group>() };
            var list = EntityBase.ParseListFromTable<Sys_Group>(table);

            //if (list.Any())
            if (list != null && list.Count > 0)
            {
                pageInfo.DataList.AddRange(list);

                pageInfo.ItemCount = itemCount;
                pageInfo.PageSize = pageInfoSearch.PageSize;
                pageInfo.PageIndex = pageInfoSearch.PageIndex;
                pageInfo.PageCount = pageCount;
            }

            return pageInfo;
        }

        public void Add(Sys_Group item)
        {
            DbCommand comm = this.GetCommand("Sp_Sys_Group_Create");

            comm.AddParameter<string>(this.Factory, "groupCode", (item.GroupCode != null && item.GroupCode.Trim().Length > 0) ? item.GroupCode.ToUpper().Trim() : null);
            comm.AddParameter<string>(this.Factory, "groupName", (item.GroupName != null && item.GroupName.Trim().Length > 0) ? item.GroupName.Trim() : null);
            comm.AddParameter<string>(this.Factory, "description", (item.Description != null && item.Description.Trim().Length > 0) ? item.Description.Trim() : null);
            comm.AddParameter<string>(this.Factory, "createBy", item.CreateBy);
            this.SafeExecuteNonQuery(comm);
        }

        public void AddMulti(DataTable table)
        {
            DbCommand comm = this.GetCommand("Sp_Sys_Group_AddMulti");

            //comm.Parameters.Add("@tblSys_Group_Type", SqlDbType.Structured).Value = (table != null && table.Rows.Count > 0) ? table : new Sys_Group().InitTable();
            //comm.Parameters["@tblSys_Group_Type"].TypeName = "MyTableType";

            comm.AddParameter<DataTable>(this.Factory, "tblSys_Group_Type", (table != null && table.Rows.Count > 0) ? table : new Sys_Group().InitTable());

            this.SafeExecuteNonQuery(comm);
        }

        public void Update(Sys_Group @new, Sys_Group old)
        {
            var item = @new;
            item.GroupCode = old.GroupCode;
            var comm = this.GetCommand("Sp_Sys_User_Update");
            if (comm == null) return;


            this.SafeExecuteNonQuery(comm);
        }

        public void Remove(Sys_Group item)
        {

        }

        public void Import(List<Sys_Group> list, bool deleteExist)
        {
        }


    }
}
