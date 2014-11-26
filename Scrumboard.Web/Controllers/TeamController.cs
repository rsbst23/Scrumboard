using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scrumboard.Web.DAL;
using System.Data.Linq;

namespace Scrumboard.Web.Controllers
{
    public class TeamController : Controller
    {
        private ScrumboardDB db = new ScrumboardDB();

        //
        // GET: /Team/

        public ActionResult Index()
        {
            return View(db.Teams.ToList());
        }

        //
        // GET: /Team/Details/5

        public ActionResult Details(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        //
        // GET: /Team/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Team/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(team);
        }

        //
        // GET: /Team/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        //
        // POST: /Team/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Team team)
        {
            if (ModelState.IsValid)
            {
                db.Entry(team).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(team);
        }

        //
        // GET: /Team/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }

        //
        // POST: /Team/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = db.Teams.Find(id);
            db.Teams.Remove(team);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult TeamMembers(int teamId)
        {
            ViewBag.TeamId = teamId;
            var dlo = new DataLoadOptions();
            dlo.LoadWith<TeamMember>(x => x.UserProfile);

            var xf = (from teamMember in db.TeamMembers
                      join userProfile in db.UserProfiles on teamMember.UserProfileId equals userProfile.UserId
                      where teamMember.TeamId == teamId
                      select userProfile).ToList();

            return PartialView(xf);
        }

        public ActionResult AddTeamMember(int teamId)
        {
            ViewBag.TeamId = teamId;
            return View(db.UserProfiles.ToList());
        }

        public ActionResult AddTeamMemberConfirmed(int id, int teamId)
        {
            db.TeamMembers.Add(new TeamMember() { TeamId = teamId, UserProfileId = id });
            db.SaveChanges();
            return RedirectToAction("Edit", new { id = teamId });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}