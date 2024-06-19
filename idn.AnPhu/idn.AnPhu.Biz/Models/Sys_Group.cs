using Client.Core.Data.Entities;
using idn.AnPhu.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Models
{
    public class Sys_Group : EntityBase
    {
        [DataColum]
        public string GroupCode { get; set; }

        [DataColum]
        public string GroupName { get; set; }

        [DataColum]
        public string Description { get; set; }

        [DataColum]
        public bool FlagActive { get; set; }

        [DataColum]
        public DateTime CreateDTime { get; set; }

        [DataColum]
        public string CreateBy { get; set; }

        [DataColum]
        public DateTime UpdateDTime { get; set; }

        [DataColum]
        public string UpdateBy { get; set; }

        public List<Sys_Permission> SysPermissions { get; set; }

        //public override void ParseData(DataRow dr)
        //{
        //    base.ParseData(dr);
        //    base.ParseDataEx(dr);
        //}

        #region["InitTable"]
        public DataTable InitTable()
        {
            var table = new DataTable(TableName.Sys_Group);
            table.Columns.Add(TblSys_Group.GroupCode, typeof(string));
            table.Columns.Add(TblSys_Group.GroupName, typeof(string));
            table.Columns.Add(TblSys_Group.Description, typeof(string));
            table.Columns.Add(TblSys_Group.FlagActive, typeof(bool));
            table.Columns.Add(TblSys_Group.CreateDTime, typeof(DateTime));
            table.Columns.Add(TblSys_Group.CreateBy, typeof(string));
            table.Columns.Add(TblSys_Group.UpdateDTime, typeof(DateTime));
            table.Columns.Add(TblSys_Group.UpdateBy, typeof(string));

            return table;
        }
        #endregion
    }
}
