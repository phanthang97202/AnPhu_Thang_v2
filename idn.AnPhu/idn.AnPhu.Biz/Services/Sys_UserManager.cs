using Client.Core.Data;
using Client.Core.Data.Entities.Paging;
using Client.Core.Utils;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using idn.AnPhu.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Services
{
    public class Sys_UserManager : DataManagerBase<Sys_User>
    {
        public Sys_UserManager(IDataProvider<Sys_User> provider)
            : base(provider)
        {
        }

        ISys_UserProvider Sys_UserProvider
        {
            get
            {
                return (ISys_UserProvider)Provider;
            }
        }

        public Sys_User Login(string userCode, string password)
        {
            var user = Get(new Sys_User() { UserCode = userCode });
            if (user != null)
            {
                string ePass = EncryptUtils.EncryptPassword(password, user.PasswordSalt);

                if (!ePass.Equals(user.Password) || !user.FlagActive)
                {
                    user = null;
                }
            }

            return user;
        }

        public List<Sys_User> GetAll()
        {
            int total = 0;
            return Provider.GetAll(0, 0, ref total);
        }

        public PageInfo<Sys_User> Search(string userCode, string fullName, string birthDayFrom, string birthDayTo, string phoneNo, string email, string sex, string flagActive, string isSysAdmin, int pageIndex, int pageCount)
        {
            int totalItems = 0;
            var pageIndexCur = pageIndex * pageCount;
            var page = new PageInfo<Sys_User>(pageIndex, pageCount);
            var list = Sys_UserProvider.Search(userCode, fullName, birthDayFrom, birthDayTo, phoneNo, email, sex, flagActive, isSysAdmin, pageIndexCur, pageCount, ref totalItems);
            page.DataList = list;
            page.ItemCount = totalItems;
            return page;
        }

        public override void Add(Sys_User item)
        {
            //item.UserId = User.GenerateNewGuid();
            //item.UserId = EntityBase.GenerateNewGuid();
            item.CreateBy = !CUtils.IsNullOrEmpty(item.CreateBy) ? CUtils.StrTrim(item.CreateBy) : "SYS";
            var salt = EncryptUtils.GenerateSalt();
            var password = EncryptUtils.EncryptPassword(item.Password, salt);

            item.Password = password.Trim();
            item.PasswordSalt = salt.Trim();
            string userCode = null;
            bool isSysAdmin = false;
            if (!CUtils.IsNullOrEmpty(item.UserCode))
            {
                userCode = CUtils.StrToUpper(item.UserCode);
                // chỉ tồn tại duy nhất 1 SysAdmin => set = false (mặc định nếu tài khoản tạo mới có usercode là SYSADMIN thì set = true)
                if (userCode.Equals("SYSADMIN"))
                {
                    isSysAdmin = true;
                }
            }
            item.FlagActive = true; // mặc định khi tạo mới tài khoản đc Active luôn
            item.IsSysAdmin = isSysAdmin;
            base.Add(item);
        }

        public void Import(List<Sys_User> list)
        {

            Sys_UserProvider.Import(list, false);

            //try
            //{
            //    #region // Init:
            //    BeginTransaction();
            //    #endregion
            //    Sys_UserProvider.Import(list, false);
            //    // Return Good:
            //    CommitTransaction();
            //}
            //catch (Exception exc)
            //{
            //    #region // Catch of try:
            //    // Return Bad:
            //    RollbackTransaction();
            //    throw exc;
            //    #endregion
            //}

        }
    }
}
