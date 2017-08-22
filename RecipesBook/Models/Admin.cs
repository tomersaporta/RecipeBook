using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace RecipesBook.Models
{
    public class Admin
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "please provide Name")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "please provide password")]
        [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        public string Password { get; set; }
    }

    public class AdminDBContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }

    }
}