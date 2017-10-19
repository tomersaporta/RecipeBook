using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesBook.Models
{
    public class Recipes
    {
        public int RecipesID { get; set; }
          
        [Required(ErrorMessage ="Name is required")]
        [Display(Name = "Recipe Name")]
        public string recipeName { get; set; }

        [Display(Name = "Type")]
        public RecipesType recipeType { get; set; }

        public enum RecipesType
        {
            MainCourse, Appetizer, Salads, Desserts
        }

        [Display(Name = "Category")]
        public RecipesCategory recipeCategory { get; set; }

        public enum RecipesCategory
        {
            Meaty, milky, Parve
        }

        [Required(ErrorMessage = "Ingredients is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Ingredients")]
        public string recipesIngredients { set; get; }

        [Required(ErrorMessage = "The recipe is required")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "The Recipe")]
        public string theRecipe { set; get; }

        [Display(Name = "Image")]
        public string recipeImage { get; set; }

        [Display(Name = "Video")]
        public string recipeVideo { get; set; }

        public int countLike { get; set; } = 0;

        public virtual ICollection<Comment> commentList { get; set; }

        public virtual User user { set; get; }


    }
}