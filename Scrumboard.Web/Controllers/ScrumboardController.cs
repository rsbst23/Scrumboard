using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scrumboard.Web.DAL;
using Scrumboard.Web.Models;
using System.Data.Linq;

namespace Scrumboard.Web.Controllers
{
    public class ScrumboardController : BaseController
    {
        //
        // GET: /Scrumboard/

        public ActionResult Index(int sprintId = 0)
        {
            ViewBag.SprintId = new SelectList(db.Sprints.Where(x => x.ProjectId == ProjectId), "Id", "Title", sprintId);

            var scrumboardModel = new ScrumboardModel();

            Dictionary<int, UserProfile> userProfiles = db.UserProfiles.ToDictionary(x => x.Id, x => x);

            Session.Add("UserProfiles", userProfiles);

            if (sprintId != 0)
            {
                scrumboardModel.BacklogItems = db.BacklogItems.Include(x => x.Tasks).Include(x => x.BacklogItemStatus).Where(x => x.SprintId == sprintId).Where(x => x.ProjectId == ProjectId).OrderBy(x => x.Priority).ToList();
            }

            return View(scrumboardModel);
        }

    }
}
