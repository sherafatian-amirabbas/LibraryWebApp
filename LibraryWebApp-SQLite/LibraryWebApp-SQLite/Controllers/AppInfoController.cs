using LibraryWebApp_SQLite.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryWebApp_SQLite.Controllers
{
    [RoutePrefix("app")]
    public class AppInfoController : Controller
    {
        DBInfoRepository _infoRepo = new DBInfoRepository();


        [Route("inf")]
        public ActionResult GetInfo()
        {
            var info = _infoRepo.GetInfo();
            return View(info);
        }
    }
}