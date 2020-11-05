﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BugReport.Commands;
using BugReport.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BugReport.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly BugTrackerContext _context;

        public ProjectRepository(BugTrackerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectItem>> GetProjectItems(ReadTasksCommand readTasksCommand)
        {
            //используется пакет X.PagedList.Mvc.Core
            var pagedList = await _context.ProjectItems.ToPagedListAsync(readTasksCommand.Page, readTasksCommand.PageSize);

            return await pagedList.ToListAsync();
        }

        public Task<ProjectItem> GetItem(int id)
        {
            return _context.ProjectItems.FindAsync(id);
        }

        public async Task<ProjectItem> CreateItem(CreateProjectItemCommand command)
        {
            ProjectItem projectItem = new ProjectItem
            {
                Created = DateTime.UtcNow,
                Description = command.Description,
                Modified = DateTime.UtcNow,
                Name = command.Name,
            };

            _context.ProjectItems.Add(projectItem);

            await _context.SaveChangesAsync();

            return projectItem;
        }

        public async Task<ProjectItem> UpdateItem(UpdateProjectItemCommand command)
        {
            ProjectItem existingItem = await _context.ProjectItems.FindAsync(command.Id);
            if (existingItem == null)
                throw new ArgumentOutOfRangeException(nameof(command.Id));

            existingItem.Name = command.Name;
            existingItem.Description = command.Description;

            existingItem.Modified = DateTime.UtcNow;

            _context.ProjectItems.Add(existingItem);
            
            _context.Entry(existingItem).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return existingItem;
        }

        public async Task DeleteItem(int id)
        {
            ProjectItem item = await _context.ProjectItems.FindAsync(id);

            if (item == null)
            {
                return;
            }

            _context.ProjectItems.Remove(item);

            await _context.SaveChangesAsync();
        }
    }
}
