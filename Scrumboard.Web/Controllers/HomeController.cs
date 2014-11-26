using Scrumboard.Web.Code;
using Scrumboard.Web.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;

namespace Scrumboard.Web.Controllers
{
    public class HomeController : Controller
    {
        private ScrumboardDB db = new ScrumboardDB();

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            Database.SetInitializer(new ScrumboardDBInitializer());

            db.TaskStatuses.Find(1);

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
