using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace gongshangchaxun.Controllers
{
    public class glController : Controller
    {
        //
        // GET: /gl/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /gl/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /gl/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /gl/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /gl/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /gl/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /gl/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /gl/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
