using Client.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Models
{
    public class Sign_In : EntityBase
    {
        [DataColum]
        [DataColumEx("UserCode")]
        //[RequireField(ErrorMessage = "Bạn phải nhập Tên đăng nhập")]
        public string UserCode { get; set; }

        [DataColum]
        public string Password { get; set; }

        //public override void ParseData(DataRow dr)
        //{
        //    base.ParseData(dr);
        //    base.ParseDataEx(dr);
        //}
    }
}
