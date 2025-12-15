using Data.models;
using Microsoft.EntityFrameworkCore;

namespace Data.context
{
    public class DataContext : DbContext
    {
        //Add-Migration message -Project Data -StartupProject ToDoList
        //Update-Database -Project Data -StartupProject ToDoList
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var cnstring = "Server=localhost\\SQLEXPRESS01;Database=ToDoListDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            //var cnstring = "Server=HOME-PC\\SQLEXPRESS;Database=ToDoListDb;Integrated Security=True;Encrypt=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(cnstring);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Lists> Lists => Set<Lists>();
        public DbSet<Tasks> Tasks => Set<Tasks>();
    }
}
