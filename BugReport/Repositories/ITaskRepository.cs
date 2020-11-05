using System.Collections.Generic;
using System.Threading.Tasks;
using BugReport.Commands;
using BugReport.Models;

namespace BugReport.Repositories
{
    public interface ITaskRepository
    {
        Task<List<TaskItem>> GetTasks(GetTasksCommand command);

        Task<TaskItem> GetTask(int id);

        Task<TaskItem> CreateTask(CreateTaskItemCommand command);

        Task<TaskItem> UpdateTask(UpdateTaskItemCommand command);

        Task DeleteTask(int id);
    }
}
