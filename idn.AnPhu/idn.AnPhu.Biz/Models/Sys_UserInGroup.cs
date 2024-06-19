using Client.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Models
{
    public class Sys_UserInGroup : EntityBase
    {
        [DataColum]
        public string UserCode { get; set; }

        [DataColum]
        public string GroupCode { get; set; }

        [DataColum]
        public DateTime CreateDTime { get; set; }

        [DataColum]
        public string CreateBy { get; set; }

        [DataEx]
        public Sys_Group SysGroup { get; set; }

        //public override void ParseData(DataRow dr)
        //{
        //    base.ParseData(dr);
        //    base.ParseDataEx(dr);
        //}

    }
}
