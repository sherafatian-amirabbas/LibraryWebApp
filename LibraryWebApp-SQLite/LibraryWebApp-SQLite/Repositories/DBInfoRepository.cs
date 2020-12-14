using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using LibraryWebApp_SQLite.DataAccess;

namespace LibraryWebApp_SQLite.Repositories
{
    public class DBInfo : IDataObject<DBInfo>
    {
        public Guid ID { get; set; }
        public string AppVersion { get; set; }
        public string DBVersion { get; set; }


        public void FromReader(SQLiteDataReader item)
        {
            this.ID = Guid.Parse(item.GetString(0));
            this.AppVersion = item.GetString(1);
            this.DBVersion = item.GetString(2);
        }
    }


    public class DBInfoRepository
    {
        internal static string _tableName = "tblDBInfo";


        #region Public Methods

        public DBInfo GetInfo()
        {
            var sqlFirst = $"select * from {_tableName} limit 1";
            var result = DBSQLiteCommand.ExecuteReader<DBInfo>(sqlFirst, null, (readerItem =>
            {
                var item = new DBInfo();
                item.FromReader(readerItem);
                return item;
            }));
            var info = result.FirstOrDefault();
            return info;
        }

        #endregion
    }
}