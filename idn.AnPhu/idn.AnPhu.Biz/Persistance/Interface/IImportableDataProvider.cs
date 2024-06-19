using Client.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace idn.AnPhu.Biz.Persistance.Interface
{
    interface IImportableDataProvider<T> : IDataProvider<T>
        where T : Client.Core.Data.Entities.EntityBase
    {
        void Import(List<T> list, bool deleteExist);
    }
}
