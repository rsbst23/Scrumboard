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
            var sprints = db.Sprints.Where(x => x.ProjectId == ProjectId).OrderBy(x => x.Title).ToList();

            if (sprintId == 0)
            {
                if (Session["SprintId"] == null)
                {
                    if (sprints.Count > 0)
                    {
                        sprintId = sprints.Last().Id;
                        Session["SprintId"] = sprintId;
                    }
                }
                else
                {
                    sprintId = Convert.ToInt32(Session["SprintId"]);
                }
            }
            else
            {
                Session["SprintId"] = sprintId;
            }

            ViewBag.SprintId = new SelectList(sprints, "Id", "Title", sprintId);

            var scrumboardModel = new ScrumboardModel();

            Dictionary<int, UserProfile> userProfiles = db.UserProfiles.ToDictionary(x => x.Id, x => x);

            scrumboardModel.TeamMembers = db.TeamMembers.Include(x => x.UserProfile).OrderBy(x => x.UserProfile.UserName);

            Session.Add("UserProfiles", userProfiles);

            if (sprintId != 0)
            {
                scrumboardModel.BacklogItems = db.BacklogItems.Include(x => x.Tasks).Include(x => x.BacklogItemStatus).Where(x => x.SprintId == sprintId).Where(x => x.ProjectId == ProjectId).OrderBy(x => x.Priority).ToList();
            }

            return View(scrumboardModel);
        }

        [HttpPost]
        public ActionResult AssignTask(int taskId, string assignedTo)
        {
            try
            {
                var userProfile = db.UserProfiles.Where(x => x.UserName == assignedTo).FirstOrDefault();

                var teamMember = db.TeamMembers.Where(x => x.UserProfileId == userProfile.Id).FirstOrDefault();

                var task = db.Tasks.Where(x => x.Id == taskId).FirstOrDefault();

                task.TeamMemberId = teamMember.Id;

                db.SaveChanges();

                var data = new { displayName = assignedTo, success = true };

                return Json(data);
            }
            catch(Exception ex)
            {
                var data = new {message = ex.ToString(), success = false };

                return Json(data);
            }
        }

    }
}
