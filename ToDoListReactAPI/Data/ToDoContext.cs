using Microsoft.EntityFrameworkCore;
using ToDoListReactApi.API.Models;

namespace ToDoListReactApi.API.Data
{
    public class ToDoContext: DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) { }

        public DbSet<ToDoTasks> ToDoTasks { get; set; }

        public DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ToDoTasks>()
                .HasOne(t => t.users) //one task has one user
                .WithMany(u => u.todotasks) //one user can have many tasks
                .HasForeignKey(t => t.userId); //foreign key in todotasks
                //.OnDelete(DeleteBehavior.Cascade);

            //seed data

            modelBuilder.Entity<Users>().HasData(
                new Users { userId = 1, name = "Maria" },
                new Users { userId = 2, name = "John" },
                new Users { userId = 3, name = "Shane" }
                );

            modelBuilder.Entity<ToDoTasks>().HasData(
                new ToDoTasks {taskId = 1, Title = "Bug 1", status="Pending", userId = 1 },
                new ToDoTasks { taskId = 2, Title = "Task 1", status = "Completed", userId = 2 },
                new ToDoTasks { taskId = 3, Title = "check emails", status = "Pending", userId = 1 },
                new ToDoTasks { taskId = 4, Title = "review documents", status = "Completed", userId = 1 },
                new ToDoTasks { taskId = 5, Title = "provide feedback", status = "Completed", userId = 2 },
                new ToDoTasks { taskId = 6, Title = "check emails", status = "Pending", userId = 2 });
        }

    }

}
