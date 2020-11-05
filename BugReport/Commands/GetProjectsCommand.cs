using System.ComponentModel.DataAnnotations;

namespace BugReport.Commands
{
    public class GetProjectsCommand
    {
        private const int DEFAULT_PAGE_SIZE = 30;
        private const int DEFAULT_PAGE_NUMBER = 1;

        [Range(0, int.MaxValue)]
        public int Page { get; set; } = DEFAULT_PAGE_NUMBER;

        [Range(1, 100)]
        public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;
    }
}
