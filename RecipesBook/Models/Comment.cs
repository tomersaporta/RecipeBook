using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace RecipesBook.Models
{
    public class Comment
    {

        public int commentID { set; get; }

        public int recipeID { set; get; }

        [Display(Name = "Comment Title")]
        [DataType(DataType.Text)]
        [Required]
        public string commentTitle { set; get; }

        [Display(Name = "Writer Name")]
        [DataType(DataType.Text)]
        [Required]
        public string writerName { set; get; }

        [Display(Name = "Content")]
        [DataType(DataType.Text)]
        [Required]
        public string content { set; get; }

        public virtual Recipes recipe { set; get; }

        

    }


}
