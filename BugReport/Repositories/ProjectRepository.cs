using System;
using System.Collections.Generic;
using System.Linq;
using BugReport.Commands;
using BugReport.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BugReport.Repositories
{
    public class ProjectRepository
    {
        private readonly BugTrackerContext _context;
        private readonly DbSet<ProjectItem> _projectItemsList;

        public ProjectRepository(BugTrackerContext context)
        {
            _context = context;
            _projectItemsList = context.ProjectItems;
        }

        public List<ProjectItem> GetProjectItems(
            int page,
            int pageSize)
        {
            IEnumerable<ProjectItem> items = _context.ProjectItems;

            //используется пакет X.PagedList.Mvc.Core
            return items.ToPagedList(page, pageSize).ToList();
        }

        public ProjectItem GetItem(int id)
        {
            return _context.ProjectItems.Find(id);
        }

        public ProjectItem CreateItem(CreateProjectItemCommand command)
        {
            ProjectItem projectItem = new ProjectItem
            {
                Created = DateTime.UtcNow,
                Description = command.Description,
                Modified = DateTime.UtcNow,
                Name = command.Name,
            };

            _projectItemsList.Add(projectItem);

            _context.SaveChanges();

            return projectItem;
        }

        public ProjectItem UpdateItem(int id, UpdateProjectItemCommand command)
        {
            ProjectItem existingItem = _context.ProjectItems.Find(id);

            existingItem.Name = command.Name;
            existingItem.Description = command.Description;

            existingItem.Modified = DateTime.UtcNow;

            _context.ProjectItems.Add(existingItem);
            
            _context.Entry(existingItem).State = EntityState.Modified;

            _context.SaveChanges();

            return existingItem;
        }

        public void DeleteItem(int id)
        {
            ProjectItem item = _context.ProjectItems.Find(id);

            _context.ProjectItems.Remove(item);

            _context.SaveChanges();
        }

        /// <summary>
        /// Начальная инициализация элементов
        /// </summary>
        public void CreateInitialElementsIfNotExist()
        {
            if (_projectItemsList.Count() == 0)
            {
                _projectItemsList.Add(new ProjectItem { Name = "Task 1", Description = "Task 1", Created = DateTime.UtcNow.AddDays(1), Modified = DateTime.UtcNow.AddDays(1) });
                _projectItemsList.Add(new ProjectItem { Name = "Task 2", Description = "Task 2", Created = DateTime.UtcNow.AddDays(-1), Modified = DateTime.UtcNow.AddDays(-1) });
                _projectItemsList.Add(new ProjectItem { Name = "Task 3", Description = "Task 3", Created = DateTime.UtcNow.AddDays(2), Modified = DateTime.UtcNow.AddDays(2) });
                _context.SaveChanges();
            }
        }
    }
}
