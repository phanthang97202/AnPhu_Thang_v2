using idn.AnPhu.Biz.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Persistance.Interface
{
    interface ISys_GroupProvider : IImportableDataProvider<Sys_Group>
    {
        void AddMulti(DataTable table);
    }
}
