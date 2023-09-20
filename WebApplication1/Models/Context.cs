using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class Context :IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }



        public Context(DbContextOptions<Context> options): base(options)
        {

        }



        public DbSet<WebApplication1.Models.CourseContent>? CourseContent { get; set; }

       }    
    }

