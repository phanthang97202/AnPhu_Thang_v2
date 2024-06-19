using idn.AnPhu.Biz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Persistance.Interface
{
    interface ISys_UserProvider : IImportableDataProvider<Sys_User>
    {
        List<Sys_User> Search(string userCode, string fullName, string birthDayFrom, string birthDayTo, string phoneNo, string email, string sex, string flagActive, string isSysAdmin, int pageIndex, int pageCount, ref int totalItems);
    }
}
