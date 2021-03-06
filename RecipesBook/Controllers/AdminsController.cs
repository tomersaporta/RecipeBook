﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RecipesBook.Models;

namespace RecipesBook.Controllers
{
    public class AdminsController : Controller
    {
        private AdminDBContext db = new AdminDBContext();
        private Book book = new Book();

        // GET: Admins
        public ActionResult Index()
        {
            //return View(db.Admins.ToList());
            return View("Index", "_AdminLayout", db.Admins.ToList());

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Admin admin)
        {
            foreach (var a in db.Admins)
            {
                if (a.Name == admin.Name && a.Password==admin.Password)
                {
                    Session["AdminID"] = a.ID.ToString();
                    Session["AdminName"] = a.Name.ToString();
                    return View("~/Views/Recipes/Management.cshtml", "_AdminLayout", book.recipes.ToList());
                }
            }
            ViewBag.msg = "userName or password not exist";
            return View();

        }

        // GET: Admins/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View("Details", "_AdminLayout", admin);
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View("Create", "_AdminLayout");
        }

        // POST: Admins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", "_AdminLayout", admin);
        }

        // GET: Admins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View("Edit", "_AdminLayout", admin);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Password")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Edit", "_AdminLayout", admin);

        }

        // GET: Admins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View("Delete", "_AdminLayout", admin);

        }

        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult ManagementRecipes()
        {
           
            return View("~/Views/Recipes/Management.cshtml", "_AdminLayout",book.recipes.ToList());

        }

        public ActionResult LogOut()
        {
            Session["AdminID"] = null;
            Session["AdminName"] = null;
            return RedirectToAction("IndexEveryOne", "Recipes");
        }
    }
}
