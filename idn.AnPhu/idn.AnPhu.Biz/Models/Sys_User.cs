using Client.Core.Data.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Models
{
    public enum UserRole : short
    {

        Member = 1,
        TrustedMember = 2,
        Moderator = 3,
        SysAdmin = 10,

    }

    public class Sys_User : EntityBase
    {
        [DataColum]
        //[DataColumEx("UserCode")]
        public string UserCode { get; set; }

        [DataColum]
        public string UserName { get; set; }

        [DataColum]
        public string FullName { get; set; }

        [DataColum]
        public string PasswordSalt { get; set; }

        [DataColum]
        public string PhoneNo { get; set; }

        [DataColum]
        public string Email { get; set; }

        [DataColum]
        public string Sex { get; set; }

        [DataColum]
        public string Avatar { get; set; }

        [DataColum]
        public DateTime BirthDay { get; set; }

        [DataColum]
        public bool FlagActive { get; set; }

        [DataColum]
        public bool IsSysAdmin { get; set; }

        [DataColum]
        public DateTime CreateDTime { get; set; }

        [DataColum]
        public string CreateBy { get; set; }

        [DataColum]
        public DateTime UpdateDTime { get; set; }

        [DataColum]
        public string UpdateBy { get; set; }

        public List<Sys_Group> SysGroups { get; set; }
        public List<Sys_Permission> SysPermissions { get; set; }
        //public List<Sys_Access> SysAccesss { get; set; }
        public List<UserRole> Roles { get; set; }

        [DataColum]
        public int TmpCount { get; set; } //???

        public bool HasPermission(params string[] permissionCodes)
        {
            var list = permissionCodes.ToList();
            if (!this.FlagActive) return false;
            if (this.IsSysAdmin) return true;
            if (this.SysPermissions == null) return false;

            return this.SysPermissions.Exists(p => list.Exists(lc => p.PermissionCode.Equals(lc, StringComparison.InvariantCultureIgnoreCase)));

        }

        // table Sign_In
        [DataColum]
        [DataColumEx("SI_Password")]
        public string Password { get; set; }

        public override void ParseData(DataRow dr)
        {
            base.ParseData(dr);
            base.ParseDataEx(dr);
        }
    }
}
