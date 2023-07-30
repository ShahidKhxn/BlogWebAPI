using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlogWebAPI.Model;

namespace BlogWebAPI.Data
{
    public class BlogWebAPIContext : DbContext
    {
        public BlogWebAPIContext (DbContextOptions<BlogWebAPIContext> options)
            : base(options)
        {
        }

        public DbSet<BlogWebAPI.Model.Post> Post { get; set; } = default!;
        public DbSet<BlogWebAPI.Model.Author> Author { get; set; } = default!;
        public DbSet<BlogWebAPI.Model.Category> Category { get; set; } = default!;


    }
}
