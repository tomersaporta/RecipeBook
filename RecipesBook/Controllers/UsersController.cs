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
        private AdminDBContext db = new AdminDBContext();
        private Book book = new Book();
        

        // GET: Users
        public ActionResult Index()
        {
            var users = book.users;
            
            return View("Index", "_AdminLayout", users.ToList());

        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User user)
        {

            //Admin Log in
            foreach (var a in db.Admins)
            {
                if (a.Name == user.FullName && a.Password == user.Password)
                {
                    Session["AdminID"] = a.ID.ToString();
                    Session["AdminName"] = a.Name.ToString();
                    return View("~/Views/Recipes/Management.cshtml", "_AdminLayout", book.recipes.ToList());
                }
            }


            //User Log in
            foreach (var u in book.users)
            {
                if(u.FullName == user.FullName && u.Password == user.Password)
                {
                    Session["userID"] = u.ID.ToString();
                    Session["userName"] = u.FullName.ToString();
                    return RedirectToAction("EditMyRecipes");
                }
            }
            ViewBag.msg2 = "User Name or Password not exist";
            return View();    

        }

        public ActionResult EditMyRecipes()
        {
            return View("EditMyRecipes", "_UserLayout", book.recipes.ToList());

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

        //admin want to see user details
        public ActionResult AdminDetails(int? id)
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
            return View("AdminDetails", "_AdminLayout", user);
        }
        //new user
        // GET: Users/Create
        public ActionResult Create()
        {
            
            return View();
        }

        public ActionResult AdminCreate()
        {

            return View("Create", "_AdminLayout");
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
                Session["userID"] = user.ID.ToString();
                Session["userName"] = user.FullName.ToString();
                return RedirectToAction("UserIndex", "Recipes");
            }

            return View(user);
        }
        //admin add new user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminCreate([Bind(Include = "ID,FullName,sex,BrithDate,password")] User user)
        {
            if (ModelState.IsValid)
            {
                book.users.Add(user);
                book.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Create", "_AdminLayout",user);
        }

        //user edit him profile
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
            return View("EditMyAccount", "_UserLayout", user);
        }


        public ActionResult EditMyAccount()
        {
            User user=null;
            if (Session["userID"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            foreach (var u in book.users)
            {
                if (u.ID.ToString() == Session["userID"].ToString())
                {
                    user = u;
                    return RedirectToAction("Edit", new { id = u.ID });
                }

            }

            return HttpNotFound();
            
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FullName,sex,BrithDate,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                Session["userName"] = user.FullName;
                book.Entry(user).State = EntityState.Modified;
                book.SaveChanges();
                return RedirectToAction("UserIndex", "Recipes");
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
            //return View(user);
            return View("Delete", "_AdminLayout", user);
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

        //GropBy Category Recipes for the charts
        public JsonResult getCategoryData()
        {

            var list = from p in book.recipes
                       group p by p.recipeCategory into g
                       select new
                       {
                           label = g.Key.ToString(),
                           quantity = g.Count()
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        //GropBy Category UesrName for the charts
        public JsonResult getNumOfRecipes()
        {

            var list = from p in book.recipes
                       group p by p.user.FullName into g
                       select new
                       {
                           label = g.Key,
                           quantity = g.Count()
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getUserData(string name)
        {

            var list = from p in book.recipes
                       where p.user.FullName == name
                       group p by p.user.FullName into g

                       select new
                       {
                           label = g.Key,
                           quantity = g.Count()
                       };
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditRecipes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = book.recipes.Find(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View("EditRecipes", "_UserLayout", recipes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRecipes([Bind(Include = "RecipesID,recipeName,recipeAuthor,recipeType,recipeCategory,recipesIngredients,theRecipe,recipeImage,recipeVideo")] Recipes recipes)
        {
            if (ModelState.IsValid)
            {
                book.Entry(recipes).State = EntityState.Modified;
                book.SaveChanges();
                return RedirectToAction("EditMyRecipes");
            }
            return View("EditRecipes", "_UserLayout", recipes);
        }

        // GET: Recipes/Delete/5
        public ActionResult DeleteRecipes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = book.recipes.Find(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }

            return View("DeleteRecipes", "_UserLayout", recipes);

        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("DeleteRecipes")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRecipesConfirmed(int id)
        {
            Recipes recipes = book.recipes.Find(id);
            book.recipes.Remove(recipes);
            book.SaveChanges();
            return RedirectToAction("EditMyRecipes");
        }

        public ActionResult DetailsRecipes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = book.recipes.Find(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View("DetailsRecipes", "_UserLayout", recipes);
        }

        public ActionResult Comments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comments = book.comment.Where(c => c.recipeID == id).ToList();
            if (comments == null)
            {
                return HttpNotFound();
            }
            return View("Comments", "_userLayout", comments);
        }

        //delete comment

        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = book.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            book.comment.Remove(comment);
            book.SaveChanges();
            var comments = book.comment.Where(c => c.recipeID == comment.recipeID).ToList();
            return View("Comments", "_userLayout",comments);
        }

        public ActionResult LogOut()
        {
            Session["UserID"] = null;
            Session["UserName"] = null;
            return RedirectToAction("IndexEveryOne", "Recipes");
        }


    }
}
