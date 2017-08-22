using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipesBook.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "please provide full name")]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        
        [EnumDataType(typeof(Sex))]
        public Sex sex { get; set; }

        public enum Sex
        {
            male, female
        }
        public DateTime BrithDate { get; set; }

        [Required(ErrorMessage ="please provide password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
        //public Roles role { get; set; }

        public virtual ICollection<Recipes> recipes { get; set; }

    }


}