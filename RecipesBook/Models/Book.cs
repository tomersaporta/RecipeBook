using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace RecipesBook.Models
{
    public class Book: DbContext
    {
        public DbSet<Recipes> recipes { get; set; }
        public DbSet<Comment> comment { get; set; }
        public DbSet<User> users { get; set; }

    }
}