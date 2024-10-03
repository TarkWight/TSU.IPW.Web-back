using Microsoft.EntityFrameworkCore;
using TSU.IPW.API.Domain.Entities;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
    }

    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<TaskList> TaskLists { get; set; }
}
