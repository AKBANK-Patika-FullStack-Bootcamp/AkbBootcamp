using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EFLibCore
{
    public class UserContext : DbContext
    {
       
        public UserContext()
        {
        }
        protected readonly IConfiguration Configuration;
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            //options.UseSqlServer(Configuration.GetConnectionString("UserDBEntities"));
            options.UseSqlServer("Data Source = localhost; Database = CustomerDB; integrated security = True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Adress>().ToTable("Adress");
            modelBuilder.Entity<APIAuthority>().ToTable("APIAuthority");
        }
        public DbSet<User> User { get; set; }
        public DbSet<Adress> Adress { get; set; }
        public DbSet<APIAuthority> APIAuthority { get; set; }
    }
}
