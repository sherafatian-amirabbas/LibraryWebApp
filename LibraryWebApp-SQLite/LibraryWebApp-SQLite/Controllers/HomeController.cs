using LibraryWebApp_SQLite.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Text;
using LibraryWebApp_SQLite.Application;


namespace LibraryWebApp_SQLite.Controllers
{
    public class HomeController : Controller
    {
        BookRepository _bookRepo = new BookRepository();

        public ActionResult Index()
        {
            var res = _bookRepo.GetBooks();
            return View(res);
        }

        public ActionResult Create(Book obj)
        {
            _bookRepo.CreateBook(obj.Title, obj.Description, obj.Abstract, obj.ISBN, obj.Author, obj.Publisher);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            _bookRepo.DeleteBook(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var book = _bookRepo.GetBook(id);

            ViewBag.Book = book;
            return View();
        }

        [HttpPost]
        public ActionResult Edit(Book obj)
        {
            _bookRepo.UpdateBook(obj.ID, obj.Title, obj.Description, obj.Abstract, obj.ISBN, obj.Author, obj.Publisher);
            return RedirectToAction("Index");
        }


        // -------------------------------- services

        [Route("~/App/Date")]
        public ActionResult AppDate()
        {
            var date = Common.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return Content(date);
        }

        [Route("~/Book/GetAll")]
        public JsonResult GetAllBooks()
        {
            var res = _bookRepo.GetBooks();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [Route("~/Book/GetAllByDateRange/{from}/{to}")]
        public JsonResult GeBooksByDateRange(DateTime from, DateTime to)
        {
            var res = _bookRepo.GeBooksByDateRange(from, to);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [Route("~/Book/GetAllByKeyword/{keyword}")]
        public JsonResult GeBooksByKeyword(string keyword)
        {
            var res = _bookRepo.GeBooksBySearchParam(keyword);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [Route("~/Book/GetByID/{id}")]
        public JsonResult GeBookByID(string id)
        {
            var res = _bookRepo.GetBook(id);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }


}