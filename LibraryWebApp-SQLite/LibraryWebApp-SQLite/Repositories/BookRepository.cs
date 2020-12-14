using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using LibraryWebApp_SQLite.Application;
using LibraryWebApp_SQLite.DataAccess;

namespace LibraryWebApp_SQLite.Repositories
{
    public class Book : IDataObject<Book>
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Abstract { get; set; }
        public string ISBN { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public DateTime CreationDate { get; set; }



        public void FromReader(SQLiteDataReader item)
        {
            this.ID = item.GetString(0);
            this.Title = item.GetString(1);
            this.Description = item.GetString(2);
            this.Abstract = item.GetString(3);
            this.ISBN = item.GetString(4);
            this.Author = item.GetString(5);
            this.Publisher = item.GetString(6);
            this.CreationDate = item.GetDateTime(7);
        }
    }


    public class BookRepository
    {
        internal static string _tableName = "tblBooks";
        internal static string _columnName_ID = "fldID";
        internal static string _columnName_Title = "fldTitle";
        internal static string _columnName_Description = "fldDescription";
        internal static string _columnName_Abstract = "fldAbstract";
        internal static string _columnName_ISBN = "fldISBN";
        internal static string _columnName_Author = "fldAuthor";
        internal static string _columnName_Publisher = "fldPublisher";
        internal static string _columnName_CreationDate = "CreationDate";


        #region Public Methods

        public void CreateBook(string Title, string Description, string Abstract, string ISBN, string Author, string Publisher)
        {

            var sqlInsert = $"insert into {_tableName} values(@id, @title, @description, @abstract, @isbn, @author, @publisher, @creationDate)";
            var parameters = new[] {
                new SQLiteParameter("@id", Guid.NewGuid().ToString()),
                new SQLiteParameter("@title", Title),
                new SQLiteParameter("@description", Description),
                new SQLiteParameter("@abstract", Abstract),
                new SQLiteParameter("@isbn", ISBN),
                new SQLiteParameter("@author", Author),
                new SQLiteParameter("@publisher", Publisher),
                new SQLiteParameter("@creationDate", Common.Now)
            };
            DBSQLiteCommand.ExecuteNonQueryCommand(sqlInsert, parameters);
        }

        public void UpdateBook(string ID, string Title, string Description, string Abstract, string ISBN, string Author, string Publisher)
        {

            var sqlUpdate = $"update {_tableName} set {_columnName_Title} = @title, {_columnName_Description} = @desc, {_columnName_Abstract} = @abs, {_columnName_ISBN} = @isbn, {_columnName_Author} = @auth, {_columnName_Publisher} = @pub where {_columnName_ID} = @id";

            var parameters = new[] {
                new SQLiteParameter("@title", Title),
                new SQLiteParameter("@desc", Description),
                new SQLiteParameter("@abs", Abstract),
                new SQLiteParameter("@isbn", ISBN),
                new SQLiteParameter("@auth", Author),
                new SQLiteParameter("@pub", Publisher),
                new SQLiteParameter("@id", ID)
            };
            DBSQLiteCommand.ExecuteNonQueryCommand(sqlUpdate, parameters);
        }

        public void DeleteBook(string id)
        {
            var sqlUpdate = $"delete from {_tableName} where {_columnName_ID} = @id";
            var parameters = new[] { new SQLiteParameter("@id", id) };
            DBSQLiteCommand.ExecuteNonQueryCommand(sqlUpdate, parameters);
        }


        public List<Book> GeBooksByDateRange(DateTime from, DateTime to)
        {
            from = from == null ? Common.Now : from;
            to = to == null ? Common.Now : to;

            from = new DateTime(from.Year, from.Month, from.Day, from.Hour, from.Minute, from.Second);
            to = new DateTime(to.Year, to.Month, to.Day, to.Hour, to.Minute, to.Second + 1);


            var allBooks = GetBooks();
            return allBooks
                .Where(u => u.CreationDate >= from && u.CreationDate <= to)
                .ToList();
        }

        public List<Book> GeBooksBySearchParam(string keyword)
        {
            keyword = keyword.ToLower();

            var allBooks = GetBooks();
            return allBooks
                .Where(u => u.Title.ToLower().Contains(keyword) || 
                            u.Description.ToLower().Contains(keyword) || 
                            u.Abstract.ToLower().Contains(keyword) || 
                            u.ISBN.ToLower().Contains(keyword) || 
                            u.Author.ToLower().Contains(keyword) || 
                            u.Publisher.ToLower().Contains(keyword))
                .ToList();
        }

        public List<Book> GetBooks()
        {
            var sqlSelect = $"select * from {_tableName}";
            var result = DBSQLiteCommand.ExecuteReader<Book>(sqlSelect, null, (readerItem =>
            {
                var item = new Book();
                item.FromReader(readerItem);
                return item;
            }));

            return result;
        }

        public Book GetBook(string id)
        {
            var sqlFirst = $"select * from {_tableName} where {_columnName_ID} = @id limit 1";
            var parameters = new SQLiteParameter[] { new SQLiteParameter("@id", id) };
            var result = DBSQLiteCommand.ExecuteReader<Book>(sqlFirst, parameters, (readerItem =>
            {
                var item = new Book();
                item.FromReader(readerItem);
                return item;
            }));
            var book = result.FirstOrDefault();
            return book;
        }

        #endregion
    }
}