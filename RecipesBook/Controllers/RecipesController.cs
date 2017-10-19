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
    public class RecipesController : Controller
    {
        public enum RecipesType
        {
            MainCourse, Appetizer, Salads, Desserts
        }
        public enum RecipesCategory
        {
            Meaty, milky, Parve
        }

        private Book db = new Book();
        private AdminDBContext admins = new AdminDBContext();


        public ActionResult Login()
        {
            return View();
        }


        // GET: Recipes
        public ActionResult IndexEveryOne(string recipeAuthor, string recipeType, string recipeCategory, string searchString)
        {

            var AuthorLst = new List<string>();

            var AuthorQry = from d in db.users
                            orderby d.FullName
                            select d.FullName;

            var TypeLst = new List<String>();

            var TypeQry = from t in db.recipes
                          orderby t.recipeType
                          select t.recipeType;

            var CategoryLst = new List<String>();

            var CategoryQry = from t in db.recipes
                              orderby t.recipeCategory
                              select t.recipeCategory;

            var recipes = from m in db.recipes
                          select m;
            
            AuthorLst.AddRange(AuthorQry.Distinct());
            ViewBag.recipeAuthor = new SelectList(AuthorLst);

            TypeLst.Add(RecipesType.Appetizer.ToString());
            TypeLst.Add(RecipesType.Desserts.ToString());
            TypeLst.Add(RecipesType.MainCourse.ToString());
            TypeLst.Add(RecipesType.Salads.ToString());
            ViewBag.recipeType = new SelectList(TypeLst);

            CategoryLst.Add(RecipesCategory.Meaty.ToString());
            CategoryLst.Add(RecipesCategory.milky.ToString());
            CategoryLst.Add(RecipesCategory.Parve.ToString());

            ViewBag.recipeCategory = new SelectList(CategoryLst);

            if (!String.IsNullOrEmpty(recipeCategory))
            {
                recipes = recipes.Where(c => c.recipeCategory.ToString().Contains(recipeCategory));
            }

            if (!String.IsNullOrEmpty(recipeType))
            {
                recipes = recipes.Where(r => r.recipeType.ToString().Contains(recipeType));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(s => s.recipeName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(recipeAuthor))
            {
                recipes = recipes.Where(x => x.user.FullName == recipeAuthor);
            }
            return View(recipes);
        }

        public ActionResult UserIndex(string recipeAuthor, string recipeType, string recipeCategory, string searchString)
        {

            var AuthorLst = new List<string>();

            var AuthorQry = from d in db.users
                            orderby d.FullName
                            select d.FullName;

            var TypeLst = new List<String>();

            var TypeQry = from t in db.recipes
                          orderby t.recipeType
                          select t.recipeType;

            var CategoryLst = new List<String>();

            var CategoryQry = from t in db.recipes
                              orderby t.recipeCategory
                              select t.recipeCategory;

            var recipes = from m in db.recipes
                          select m;

            AuthorLst.AddRange(AuthorQry.Distinct());
            ViewBag.recipeAuthor = new SelectList(AuthorLst);

            TypeLst.Add(RecipesType.Appetizer.ToString());
            TypeLst.Add(RecipesType.Desserts.ToString());
            TypeLst.Add(RecipesType.MainCourse.ToString());
            TypeLst.Add(RecipesType.Salads.ToString());
            ViewBag.recipeType = new SelectList(TypeLst);

            CategoryLst.Add(RecipesCategory.Meaty.ToString());
            CategoryLst.Add(RecipesCategory.milky.ToString());
            CategoryLst.Add(RecipesCategory.Parve.ToString());

            ViewBag.recipeCategory = new SelectList(CategoryLst);

            if (!String.IsNullOrEmpty(recipeCategory))
            {
                recipes = recipes.Where(c => c.recipeCategory.ToString().Contains(recipeCategory));
            }

            if (!String.IsNullOrEmpty(recipeType))
            {
                recipes = recipes.Where(r => r.recipeType.ToString().Contains(recipeType));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(s => s.recipeName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(recipeAuthor))
            {
                recipes = recipes.Where(x => x.user.FullName == recipeAuthor);
            }

            // return View(recipes);
            return View("Index", "_UserLayout", recipes);
        }

        public ActionResult AdminIndex(string recipeAuthor, string recipeType, string recipeCategory, string searchString)
        {

            var AuthorLst = new List<string>();

            var AuthorQry = from d in db.users
                            orderby d.FullName
                            select d.FullName;

            var TypeLst = new List<String>();

            var TypeQry = from t in db.recipes
                          orderby t.recipeType
                          select t.recipeType;

            var CategoryLst = new List<String>();

            var CategoryQry = from t in db.recipes
                              orderby t.recipeCategory
                              select t.recipeCategory;

            var recipes = from m in db.recipes
                          select m;

            AuthorLst.AddRange(AuthorQry.Distinct());
            ViewBag.recipeAuthor = new SelectList(AuthorLst);

            TypeLst.Add(RecipesType.Appetizer.ToString());
            TypeLst.Add(RecipesType.Desserts.ToString());
            TypeLst.Add(RecipesType.MainCourse.ToString());
            TypeLst.Add(RecipesType.Salads.ToString());
            ViewBag.recipeType = new SelectList(TypeLst);

            CategoryLst.Add(RecipesCategory.Meaty.ToString());
            CategoryLst.Add(RecipesCategory.milky.ToString());
            CategoryLst.Add(RecipesCategory.Parve.ToString());

            ViewBag.recipeCategory = new SelectList(CategoryLst);

            if (!String.IsNullOrEmpty(recipeCategory))
            {
                recipes = recipes.Where(c => c.recipeCategory.ToString().Contains(recipeCategory));
            }

            if (!String.IsNullOrEmpty(recipeType))
            {
                recipes = recipes.Where(r => r.recipeType.ToString().Contains(recipeType));
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                recipes = recipes.Where(s => s.recipeName.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(recipeAuthor))
            {
                recipes = recipes.Where(x => x.user.FullName == recipeAuthor);
            }
            //return View(recipes);
            return View("Index", "_AdminLayout", recipes);
        }

        public ActionResult Management()
        {
          //  return View(db.recipes.ToList());
            return View("Management", "_AdminLayout", db.recipes.ToList());
        }

        public ActionResult ManagementMyRecipes()
        {
            if (Session["userID"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var recipes = (from r in db.recipes
                           join u in db.users on r.user.ID equals u.ID
                           select r);

            recipes = recipes.Where(x => x.user.FullName== Session["userName"].ToString());

            return View("Management","_UserLayout",recipes);


        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = db.recipes.Find(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View("Details", "_AdminLayout", recipes);
            //return View(recipes);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            
            var tempNames = new List<string>();
            foreach (var a in db.users)
            {
                var name = a.FullName;
                tempNames.Add(name);
            }
            
            
            ViewBag.recipeAuthor = new SelectList(tempNames);

            return View("Create","_UserLayout");
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecipesID,recipeName,recipeAuthor,recipeType,recipeCategory,recipesIngredients,theRecipe,recipeImage,recipeVideo")] Recipes recipes)
        {
            var tempNames = new List<string>();
            foreach (var a in db.users)
            {
                var name = a.FullName;
                tempNames.Add(name);
            }

            ViewBag.recipeAuthor = new SelectList(tempNames);

            foreach (var user in db.users)
                if (user.FullName.ToString() == Session["userName"].ToString())
                    recipes.user = user;
   
            if (ModelState.IsValid)
            {
                db.recipes.Add(recipes);
                db.SaveChanges();
                return RedirectToAction("UserIndex");
            }

           // return View(recipes);
            return View("Create", "_UserLayout",recipes);
        }

        // GET: Recipes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = db.recipes.Find(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
          //  return View(recipes);
            return View("Edit", "_AdminLayout", recipes);
        }
        //add like 
        public ActionResult AddLike(int? id)
        {
            Recipes recipes = db.recipes.Find(id);
            recipes.countLike++;
            db.SaveChanges();

            if (Session["userID"]!=null)
            {
                return RedirectToAction("userIndex");
            }
            if (Session["AdminID"] != null)
            {
                return RedirectToAction("AdminIndex");
            }

            
            return RedirectToAction("IndexEveryOne");
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecipesID,recipeName,recipeAuthor,recipeType,recipeCategory,recipesIngredients,theRecipe,recipeImage,recipeVideo")] Recipes recipes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recipes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Management");
            }
            return View(recipes);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipes recipes = db.recipes.Find(id);
            if (recipes == null)
            {
                return HttpNotFound();
            }
            return View("Delete", "_AdminLayout", recipes);
           // return View(recipes);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipes recipes = db.recipes.Find(id);
            db.recipes.Remove(recipes);
            db.SaveChanges();
            return RedirectToAction("Management");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // POST: /Recipes/AddComment
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment([Bind(Include = "commentID,recipeID,commentTitle,writerName,content,recipes")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                if (Session["userID"] != null)
                {
                    comment.writerName = Session["userName"].ToString();
                }
                if (Session["AdminID"] != null)
                {
                    comment.writerName = Session["AdminName"].ToString();
                }

                db.comment.Add(comment);
                Recipes recipe = db.recipes.Find(comment.recipeID);
                recipe.commentList.Add(comment);
                db.SaveChanges();
                if (Session["userID"] != null)
                {
                    return RedirectToAction("UserIndex", "recipes");
                }
                if (Session["AdminID"] != null)
                {
                    return RedirectToAction("AdminIndex", "recipes");
                }
                return RedirectToAction("UserIndex", "recipes");
            }

            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminAddComment([Bind(Include = "commentID,recipeID,commentTitle,writerName,content,recipes")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.comment.Add(comment);
                Recipes recipe = db.recipes.Find(comment.recipeID);
                recipe.commentList.Add(comment);
                db.SaveChanges();
                return RedirectToAction("AdminIndex", "recipes");
            }

            return View(comment);
        }



        public ActionResult Comments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comments = db.comment.Where(c => c.recipeID == id).ToList();
            if (comments == null)
            {
                return HttpNotFound();
            }
            
            return View("~/Views/Recipes/Comments.cshtml", "_UserLayout", comments);
        }

        public ActionResult AdminComments(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var comments = db.comment.Where(c => c.recipeID == id).ToList();
            if (comments == null)
            {
                return HttpNotFound();
            }

            return View("~/Views/Recipes/Comments.cshtml", "_AdminLayout", comments);
        }


        //delete comment

        public ActionResult DeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            db.comment.Remove(comment);
            db.SaveChanges();
            var comments = db.comment.Where(c => c.recipeID == comment.recipeID).ToList();
            return View("~/Views/Users/Comments.cshtml", "_UserLayout", comments);
        }

        public ActionResult AdminDeleteComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var comment = db.comment.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            db.comment.Remove(comment);
            db.SaveChanges();
            var comments = db.comment.Where(c => c.recipeID == comment.recipeID).ToList();
            return View("~/Views/Recipes/Comments.cshtml", "_AdminLayout", comments);
        }


    }
}
