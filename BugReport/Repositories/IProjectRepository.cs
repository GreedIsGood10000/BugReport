using System.Collections.Generic;
using System.Threading.Tasks;
using BugReport.Commands;
using BugReport.Models;

namespace BugReport.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectItem>> GetProjects(GetProjectsCommand getProjectsCommand);

        Task<ProjectItem> GetProject(int id);

        Task<ProjectItem> CreateProject(CreateProjectItemCommand command);

        Task<ProjectItem> UpdateProject(UpdateProjectItemCommand command);

        Task DeleteProject(int id);
    }
}
