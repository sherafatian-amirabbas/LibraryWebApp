using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace LibraryWebApp_SQLite.Repositories
{
    public interface IDataObject<T> where T : class, new()
    {
        void FromReader(SQLiteDataReader item);
    }
}