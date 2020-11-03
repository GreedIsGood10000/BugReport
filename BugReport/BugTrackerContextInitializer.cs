using System;
using BugReport.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace BugReport
{
    public class BugTrackerContextInitializer
    {
        public void Initialize(BugTrackerContext context)
        {
            if (!context.ProjectItems.Any())
            {
                context.Add(new ProjectItem { Name = "Task 1", Description = "Task 1", Created = DateTime.UtcNow.AddDays(1), Modified = DateTime.UtcNow.AddDays(1) });
                context.Add(new ProjectItem { Name = "Task 2", Description = "Task 2", Created = DateTime.UtcNow.AddDays(-1), Modified = DateTime.UtcNow.AddDays(-1) });
                context.Add(new ProjectItem { Name = "Task 3", Description = "Task 3", Created = DateTime.UtcNow.AddDays(2), Modified = DateTime.UtcNow.AddDays(2) });
            }

            if (!context.TaskItems.Any())
            {
                context.Add(new TaskItem { Name = "Task 1", Description = "Task 1", Priority = TaskItem.TaskPriority.High, Created = DateTime.Now.AddDays(1), Modified = DateTime.UtcNow.AddDays(1) });
                context.Add(new TaskItem { Name = "Task 2", Description = "Task 2", Priority = TaskItem.TaskPriority.Low, Created = DateTime.Now.AddDays(-1), Modified = DateTime.UtcNow.AddDays(-1) });
                context.Add(new TaskItem { Name = "Task 3", Description = "Task 3", Priority = TaskItem.TaskPriority.Medium, Created = DateTime.Now.AddDays(2), Modified = DateTime.UtcNow.AddDays(2) });
            }

            context.SaveChanges();
        }
    }
}
