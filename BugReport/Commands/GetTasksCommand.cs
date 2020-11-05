using System;
using BugReport.Models;

namespace BugReport.Commands
{
    public class GetTasksCommand
    {
        public string SortOrder { get; set; }

        public int[] ProjectId { get; set; }

        public DateTime? DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public TaskItem.TaskStatus[] Status { get; set; }

        public TaskItem.TaskPriority[] Priority { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
