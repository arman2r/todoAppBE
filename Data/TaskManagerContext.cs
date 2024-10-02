using Microsoft.EntityFrameworkCore;
using todoListApp.Models;

public class TaskManagerContext : DbContext
{
    public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
    {
    }

    public DbSet<TaskTodo> Task { get; set; }
    public DbSet<Person> Person { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskTodo>()
            .HasKey(t => t.TaskId); // Asegúrate de que TaskId sea la clave primaria
    }
}
