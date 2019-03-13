using System;
using System.ComponentModel.DataAnnotations;

namespace BugReport.Models
{
    public class TaskItem
    {
        public const int TaskItemNameLenght = 50;
        public const int TaskItemDescriptionLenght = 500;

        public enum TaskStatus
        {
            New, InWork, Closed
        }

        public enum TaskPriority
        {
            Low, Medium, High
        }
        
        public int ID { get; set; }

        public int ProjectID { get; set; }

        public ProjectItem ProjectItem { get; set; }

        [StringLength(TaskItemNameLenght)]
        public string Name { get; set; }

        [StringLength(TaskItemDescriptionLenght)]
        public string Description { get; set; }

        public TaskPriority Priority { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Modified { get; set; }

        public TaskStatus Status { get; set; }
    }
}