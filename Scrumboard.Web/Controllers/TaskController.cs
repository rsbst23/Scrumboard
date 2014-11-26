using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scrumboard.Web.DAL;

namespace Scrumboard.Web.Controllers
{
    public class TaskController : Controller
    {
        private ScrumboardDB db = new ScrumboardDB();

        //
        // GET: /Task/

        public ActionResult Index(int backlogItemId)
        {
            var tasks = db.Tasks.Include(t => t.TaskStatus).Include(t => t.TeamMember).Where(t => t.BacklogItemId == backlogItemId);
            return PartialView(tasks.ToList());
        }

        //
        // GET: /Task/Details/5

        public ActionResult Details(int id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        //
        // GET: /Task/Create

        public ActionResult Create(int backlogItemId)
        {
            ViewBag.TaskStatusId = new SelectList(db.TaskStatuses, "Id", "Title", 1);
            ViewBag.TeamMemberId = new SelectList(db.TeamMembers, "Id", "Id");
            ViewBag.BacklogItemId = backlogItemId;
            ViewBag.RefreshParent = false;

            return PartialView();
        }

        //
        // POST: /Task/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Tasks.Add(task);
                db.SaveChanges();
                ViewBag.RefreshParent = true;
            }

            ViewBag.TaskStatusId = new SelectList(db.TaskStatuses, "Id", "Title", task.TaskStatusId);
            ViewBag.TeamMemberId = new SelectList(db.TeamMembers, "Id", "Id", task.TeamMemberId);
            return PartialView(task);
        }

        //
        // GET: /Task/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaskStatusId = new SelectList(db.TaskStatuses, "Id", "Title", task.TaskStatusId);
            ViewBag.TeamMemberId = new SelectList(db.TeamMembers, "Id", "Id", task.TeamMemberId);
            ViewBag.RefreshParent = false;
            return PartialView(task);
        }

        //
        // POST: /Task/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.RefreshParent = true;
            }

            ViewBag.TaskStatusId = new SelectList(db.TaskStatuses, "Id", "Title", task.TaskStatusId);
            ViewBag.TeamMemberId = new SelectList(db.TeamMembers, "Id", "Id", task.TeamMemberId);
            return PartialView(task);
        }

        //
        // GET: /Task/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Task task = db.Tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        //
        // POST: /Task/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return RedirectToAction("Edit", "BacklogItem", new { id = task.BacklogItemId });
        }

        [HttpPost, ActionName("DeleteTaskFromScrumboard")]
        public ActionResult DeleteTaskFromScrumboard(int id)
        {
            Task task = db.Tasks.Find(id);
            db.Tasks.Remove(task);
            db.SaveChanges();
            return Json(true);
        }

        public ActionResult EditTaskStatus(int id, string status)
        {
            Task task = db.Tasks.Find(id);

            switch (status)
            {
                case "To Do": task.TaskStatusId = 1; break;
                case "In Progress": task.TaskStatusId = 2; break;
                case "Done": task.TaskStatusId = 3; break;
            }

            db.SaveChanges();

            return Json(true);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}