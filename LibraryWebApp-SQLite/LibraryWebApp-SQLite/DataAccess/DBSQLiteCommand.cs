using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;

namespace LibraryWebApp_SQLite.DataAccess
{
    public class DBSQLiteCommand
    {
        public static void ExecuteNonQueryCommand(string sqlCommand, SQLiteParameter[] parameters)
        {
            var conn = DBSQLiteConnection.CreateOrGet();
            var comm = conn.CreateCommand(sqlCommand, parameters);
            conn.OpenConnection();

            try
            {
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.CloseConnection();
            }
        }

        public static List<T> ExecuteReader<T>(string sqlCommand, SQLiteParameter[] parameters, Func<SQLiteDataReader, T> converter)
        {
            var conn = DBSQLiteConnection.CreateOrGet();
            var comm = conn.CreateCommand(sqlCommand, parameters);

            conn.OpenConnection();

            try
            {
                SQLiteDataReader dataReader = comm.ExecuteReader();

                List<T> res = new List<T>();
                while (dataReader.Read())
                {
                    res.Add(converter(dataReader));
                }

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                conn.CloseConnection();
            }
        }
    }
}