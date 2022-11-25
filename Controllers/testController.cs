using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using gongshangchaxun.Models;
using gongshangchaxun.DAL;

namespace gongshangchaxun.Controllers
{
    public class testController : Controller
    {
        private GongshangContent db = new GongshangContent();

        //
        // GET: /test/
        [Authorize(Roles="manage")]
        public ActionResult Index()
        {
            return View(db.setupdbs.ToList());
        }

        //
        // GET: /test/Details/5
         [Authorize(Roles = "manage")]
        public ActionResult Details(string id = null)
        {
            setupdb setupdb = db.setupdbs.Find(id);
            if (setupdb == null)
            {
                return HttpNotFound();
            }
            return View(setupdb);
        }

        //
        // GET: /test/Create
         [Authorize(Roles = "manage")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /test/Create

        [HttpPost]
        [Authorize(Roles = "manage")]
        public ActionResult Create(setupdb setupdb)
        {
            if (ModelState.IsValid)
            {
                db.setupdbs.Add(setupdb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(setupdb);
        }

        //
        // GET: /test/Edit/5
         [Authorize(Roles = "manage")]
        public ActionResult Edit(string id = null)
        {
            setupdb setupdb = db.setupdbs.Find(id);
            if (setupdb == null)
            {
                return HttpNotFound();
            }
            ViewBag.canshu = setupdb.canshu;
            return View(setupdb);
        }

        //
        // POST: /test/Edit/5

        [HttpPost]
        [Authorize(Roles = "manage")]
        public ActionResult Edit(setupdb setupdb)
        {
            if (ModelState.IsValid)
            {
                db.Entry(setupdb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(setupdb);
        }

        //
        // GET: /test/Delete/5
 [Authorize(Roles = "manage")]
        public ActionResult Delete(string id = null)
        {
            setupdb setupdb = db.setupdbs.Find(id);
            if (setupdb == null)
            {
                return HttpNotFound();
            }
            return View(setupdb);
        }

        //
        // POST: /test/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            setupdb setupdb = db.setupdbs.Find(id);
            db.setupdbs.Remove(setupdb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}