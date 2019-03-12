using System.ComponentModel.DataAnnotations;
using BugReport.Models;

namespace BugReport.Commands
{
    public class CreateProjectItemCommand
    {
        [StringLength(ProjectItem.ProjectItemNameLenght)]
        public string Name { get; set; }

        [StringLength(ProjectItem.ProjectItemDescriptionLenght)]
        public string Description { get; set; }
    }
}
