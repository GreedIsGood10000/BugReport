using System.Collections.Generic;
using System.Threading.Tasks;
using BugReport.Commands;
using BugReport.Models;

namespace BugReport.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectItem>> GetProjectItems(int page, int pageSize);

        Task<ProjectItem> GetItem(int id);

        Task<ProjectItem> CreateItem(CreateProjectItemCommand command);

        Task<ProjectItem> UpdateItem(int id, UpdateProjectItemCommand command);

        Task DeleteItem(int id);

        Task CreateInitialElementsIfNotExist();
    }
}
