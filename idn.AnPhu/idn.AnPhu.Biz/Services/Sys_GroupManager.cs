using Client.Core.Data;
using idn.AnPhu.Biz.Models;
using idn.AnPhu.Biz.Persistance.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Services
{
    public class Sys_GroupManager : DataManagerBase<Sys_Group>
    {
        public Sys_GroupManager(IDataProvider<Sys_Group> provider)
            : base(provider)
        {
        }

        ISys_GroupProvider Sys_GroupProvider
        {
            get
            {
                return (ISys_GroupProvider)Provider;
            }
        }

        public List<Sys_Group> GetAll()
        {
            int total = 0;
            return Provider.GetAll(0, 0, ref total);
        }

        public override void Add(Sys_Group item)
        {
            item.FlagActive = true;
            base.Add(item);
        }

        public void AddMulti(DataTable table)
        {
            Sys_GroupProvider.AddMulti(table);
        }

    }
}
