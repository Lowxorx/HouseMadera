using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.DAL
{
    public interface IDbConnection : IDisposable
    {
        void Open();
        DbCommand GetCommand();
        DbDataAdapter GetAdapter();
        DbParameter GetDbParameter(string name, object value);
        DbDataReader GetDataReader(DbCommand command);
    }
}

