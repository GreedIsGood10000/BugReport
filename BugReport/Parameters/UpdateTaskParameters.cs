using System.ComponentModel.DataAnnotations;
using BugReport.Models;

namespace BugReport.Parameters
{
    public class UpdateTaskParameters
    {
        public int ProjectId { get; set; }

        [StringLength(TaskItem.TaskItemNameLenght)]
        public string Name { get; set; }

        [StringLength(TaskItem.TaskItemDescriptionLenght)]
        public string Description { get; set; }

        public TaskItem.TaskPriority Priority { get; set; }

        public TaskItem.TaskStatus Status { get; set; }
    }
}
