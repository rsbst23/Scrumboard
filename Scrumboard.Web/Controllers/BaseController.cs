using Scrumboard.Web.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scrumboard.Web.Controllers
{
    public class BaseController : Controller
    {
        public ScrumboardDB db = new ScrumboardDB();

        public int ProjectId
        {
            get
            {
                return Convert.ToInt32(Session["ProjectId"]);
            }
            set
            {
                Session.Add("ProjectId", value);
                Project = db.Projects.Find(value);
            }
        }

        public Project Project
        {
            get
            {
                if (Session["Project"] != null)
                {
                    return (Project)Session["Project"];
                }

                return null;
            }
            set
            {
                Session.Add("Project", value);
            }
        }
    }
}