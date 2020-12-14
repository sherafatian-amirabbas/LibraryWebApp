using LibraryWebApp_SQLite.Application;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;


namespace LibraryWebApp_SQLite.DataAccess
{
    public class DBSQLiteConnection
    {
        static readonly string _PATH = HttpContext.Current.Server.MapPath("~/App_Data/") + "SQLiteDB.db";

        readonly SQLiteConnection _connection = new SQLiteConnection($"Data Source = {_PATH};");


        #region Properties

        public bool IsOpen { get; set; }

        #endregion


        #region Constructors

        private DBSQLiteConnection() { }

        #endregion


        #region Public Methods

        public void OpenConnection()
        {
            try
            {
                if (!this.IsOpen)
                {
                    _connection.Open();
                    this.IsOpen = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public void CloseConnection()
        {
            try
            {
                if (this.IsOpen)
                {
                    _connection.Close();
                    this.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }

        public SQLiteCommand CreateCommand(string sqlCommand, params SQLiteParameter[] parameters)
        {
            var command = _connection.CreateCommand();
            command.CommandText = sqlCommand;

            if (parameters != null && parameters.Length != 0)
            {
                foreach (var item in parameters)
                    command.Parameters.Add(item);
            }

            return command;
        }

        #endregion


        static object _OBJECT_LOCK = new object();
        public static DBSQLiteConnection CreateOrGet()
        {
            var ins = SQLiteConnectionFactory.Restore();
            if (ins == null)
            {
                lock (_OBJECT_LOCK)
                {
                    ins = SQLiteConnectionFactory.Restore();
                    if (ins == null)
                    {
                        ins = new DBSQLiteConnection();
                        SQLiteConnectionFactory.Cache(ins);
                    }
                }
            }

            return ins;
        }

        private static class SQLiteConnectionFactory
        {
            static readonly string _key;
            static CachFactory<DBSQLiteConnection> _FACTORY = new CachFactory<DBSQLiteConnection>();


            #region Constructor

            static SQLiteConnectionFactory()
            {
                _key = string.Format("__SQLiteConnection_{0}__", Guid.NewGuid());
            }

            #endregion


            #region Override Methods

            public static object Cache(DBSQLiteConnection obj)
            {
                _FACTORY.SetDataSource(HttpContext.Current.Items);
                return _FACTORY.Cache(obj, _key);
            }

            public static DBSQLiteConnection Restore()
            {
                _FACTORY.SetDataSource(HttpContext.Current.Items);
                return _FACTORY.Restore(_key);
            }

            public static DBSQLiteConnection Flush()
            {
                _FACTORY.SetDataSource(HttpContext.Current.Items);
                return _FACTORY.Flush(_key);
            }

            #endregion
        }
    }
}