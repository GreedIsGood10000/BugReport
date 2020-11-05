using System.Collections.Generic;
using System.Threading.Tasks;
using BugReport.Commands;
using BugReport.Models;

namespace BugReport.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectItem>> GetProjectItems(ReadTasksCommand readTasksCommand);

        Task<ProjectItem> GetItem(int id);

        Task<ProjectItem> CreateItem(CreateProjectItemCommand command);

        Task<ProjectItem> UpdateItem(UpdateProjectItemCommand command);

        Task DeleteItem(int id);
    }
}
