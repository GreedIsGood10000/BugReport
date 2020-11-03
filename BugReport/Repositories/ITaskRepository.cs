using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugReport.Commands;
using BugReport.Models;

namespace BugReport.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetTaskItems(string sortOrder,
            int[] projectId,
            DateTime? dateFrom,
            DateTime? dateTo,
            TaskItem.TaskStatus[] status,
            TaskItem.TaskPriority[] priority,
            int page,
            int pageSize);

        Task<TaskItem> GetItem(int id);

        Task<TaskItem> CreateItem(CreateTaskItemCommand command);

        Task<TaskItem> UpdateItem(int id, UpdateTaskItemCommand command);

        Task DeleteItem(int id);
    }
}
