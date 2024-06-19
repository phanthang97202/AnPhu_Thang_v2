using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Services;

namespace idn.AnPhu.Website.State
{
    public class UserState
    {
        private Sys_UserManager Sys_UserManager
        {
            get { return ServiceFactory.Sys_UserManager; }
        }

        public Sys_User SysUser { get; set; }
        public UserState(Sys_User user)
        {

            var listRoles = new List<UserRole>();
            UserName = user.UserCode;
            IsSysAdmin = user.IsSysAdmin;
            //ListAccess = user.ListAccess;
            // mặc định gán
            listRoles.Add(UserRole.TrustedMember);
            Role = UserRole.TrustedMember;
            if (IsSysAdmin) {
                // nếu có quyền SysAdmin => gán bằng quyền SysAdmin
                listRoles.Add(UserRole.SysAdmin);
                Role = UserRole.SysAdmin;
            }
            if (listRoles.Count > 0)
            {
                Roles = new List<UserRole>();
                Roles.AddRange(listRoles);
                user.Roles = new List<UserRole>();
                user.Roles.AddRange(listRoles);
            }
            this.SysUser = user;
        }

        public List<UserRole> Roles { get; set; }

        public UserRole Role
        {
            get;
            set;
        }

        //public List<SysAccess> ListAccess { get; set; }

        public IEnumerable<string> AllRoleNames
        {
            get
            {
                return this.Roles.Select(role => role.ToString());
            }
        }

        public string UserName
        {
            get;
            set;
        }


        public bool IsSysAdmin
        {
            get;
            set;
        }
    }
}