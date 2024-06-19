using Client.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Models
{
    public class Sys_Permission : EntityBase
    {
        [DataColum]
        public string PermissionCode { get; set; }

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

        //public override void ParseData(DataRow dr)
        //{
        //    base.ParseData(dr);
        //    base.ParseDataEx(dr);
        //}
    }
}
