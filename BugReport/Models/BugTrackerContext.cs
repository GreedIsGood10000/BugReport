using Microsoft.EntityFrameworkCore;

namespace BugReport.Models
{
    public class BugTrackerContext : DbContext
    {
        public BugTrackerContext(DbContextOptions<BugTrackerContext> options) : base(options)
        {
            
        }

        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<ProjectItem> ProjectItems { get; set; }
    }
}
