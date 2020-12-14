using Newtonsoft.Json;
using LibraryWebApp_SQLite.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LibraryWebApp_SQLite.Application
{
    public class Common
    {
        public static DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}