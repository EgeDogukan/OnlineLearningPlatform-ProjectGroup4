using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Context : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Assignment> Assignment { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }



        public Context(DbContextOptions<Context> options): base(options)
        {

        }

       }
    }

