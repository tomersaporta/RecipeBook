using System;
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
    public class UsersController : Controller
    {
        private Book book = new Book();
        

        // GET: Users
        public ActionResult Index()
        {

            if (Session["userID"] != null)
            {
                return View();

            }
            else
                return RedirectToAction("Create");

            //var users = book.users;
            //return View(users.ToList());
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {
            foreach (var u in book.users)
            {
                if(u.FullName == user.FullName && u.Password == user.Password)
                {
                    Session["userID"] = u.ID.ToString();
                    Session["userName"] = u.FullName.ToString();
                    return RedirectToAction("Index");
                }
            }

            return View();    

        }

        public ActionResult LoggedIn()
        {
            if (Session["userID"] != null)
                return View();
            else
                return RedirectToAction("Login");
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = book.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FullName,sex,BrithDate,password")] User user)
        {
         
            
            if (ModelState.IsValid)
            {
                book.users.Add(user);
                book.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = book.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,roleID,FirstName,LastName,sex,BrithDate")] User user)
        {
            if (ModelState.IsValid)
            {
                book.Entry(user).State = EntityState.Modified;
                book.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = book.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = book.users.Find(id);
            book.users.Remove(user);
            book.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                book.Dispose();
            }
            base.Dispose(disposing);
        }


        public JsonResult getCategoryData()
        {

            Dictionary<string, int> userRecipesCount = new Dictionary<string, int>();
            List<object> list = new List<object>();
            int num;
            foreach (var recipe in book.recipes)
            {
                if (userRecipesCount.ContainsKey(recipe.recipeCategory.ToString()))
                {
                    num = userRecipesCount[recipe.recipeCategory.ToString()];
                    userRecipesCount[recipe.recipeCategory.ToString()] = num + 1;
                }
                else
                    userRecipesCount.Add(recipe.recipeCategory.ToString(), 1);

            }
            foreach (var user in userRecipesCount)
            {
                list.Add(new { label = user.Key, quantity = user.Value });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getNumOfRecipes()
        {

            Dictionary<string, int> userRecipesCount = new Dictionary<string, int>();
            List<object> list = new List<object>();
            int num;
            foreach (var recipe in book.recipes)
            {
                if (userRecipesCount.ContainsKey(recipe.recipeAuthor.ToString()))
                {
                    num = userRecipesCount[recipe.recipeAuthor.ToString()];
                    userRecipesCount[recipe.recipeAuthor.ToString()] = num + 1;
                }
                else
                    userRecipesCount.Add(recipe.recipeAuthor.ToString(), 1);

            }
            foreach (var user in userRecipesCount)
            {
                list.Add(new { label = user.Key, quantity = user.Value });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getUserData(string id)
        {

            Dictionary<string, int> userRecipesCount = new Dictionary<string, int>();
            List<object> list = new List<object>();
            int num;
            foreach (var user in book.users)
            {
                if (user.ID.ToString()== id)
                {
                    foreach (var recipe in user.recipes)
                        if (userRecipesCount.ContainsKey(recipe.recipeCategory.ToString()))
                        {
                            num = userRecipesCount[recipe.recipeCategory.ToString()];
                            userRecipesCount[recipe.recipeCategory.ToString()] = num + 1;
                        }
                        else
                            userRecipesCount.Add(recipe.recipeCategory.ToString(), 1);
                }
            }
            foreach (var user in userRecipesCount)
            {
                list.Add(new { label = user.Key, quantity = user.Value });
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }



       
         



    }
}
