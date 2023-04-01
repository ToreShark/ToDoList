using Microsoft.EntityFrameworkCore;
namespace ToDoList.Data;

public class TDLContext : DbContext
{
    public TDLContext(DbContextOptions<TDLContext> options) : base(options)
    {
    }

    public DbSet<ToDoList.Models.TaskEntity> Tasks { get; set; }
}