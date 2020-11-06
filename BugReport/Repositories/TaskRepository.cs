using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugReport.Commands;
using BugReport.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BugReport.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly BugTrackerContext _context;

        public TaskRepository(BugTrackerContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetTasks(GetTasksCommand command)
        {
            IEnumerable<TaskItem> items = _context.TaskItems;

            switch (command.SortOrder)
            {
                case "priority":
                    items = items.OrderBy(x => x.Priority);
                    break;
                case "created":
                    items = items.OrderBy(x => x.Created);
                    break;
            }

            if (command.ProjectId?.Length != 0)
            {
                items = items.Where(x => command.ProjectId.Contains(x.ProjectID));
            }

            if (command.Status?.Length != 0)
            {
                items = items.Where(x => command.Status.Contains(x.Status));
            }

            if (command.Priority?.Length != 0)
            {
                items = items.Where(x => command.Priority.Contains(x.Priority));
            }

            if (command.DateFrom != null)
            {
                items = items.Where(x => x.Created >= command.DateFrom);
            }

            if (command.DateTo != null)
            {
                items = items.Where(x => x.Created <= command.DateTo);
            }

            //используется пакет X.PagedList.Mvc.Core
            var itemsPagedList = await items.ToPagedListAsync(command.Page, command.PageSize);

            return await itemsPagedList.ToListAsync();
        }

        public Task<TaskItem> GetTask(int id)
        {
            return _context.TaskItems.FindAsync(id);
        }

        public async Task<TaskItem> CreateTask(CreateTaskItemCommand command)
        {
            var taskItem = new TaskItem
            {
                Created = DateTime.UtcNow,
                Modified = DateTime.UtcNow,
                ProjectID = command.ProjectID,
                Description = command.Description,
                Name = command.Name,
                Priority = command.Priority,
                Status = TaskItem.TaskStatus.New
            };

            _context.TaskItems.Add(taskItem);

            await _context.SaveChangesAsync();

            return taskItem;
        }

        public async Task<TaskItem> UpdateTask(UpdateTaskItemCommand command)
        {
            var existingItem = await _context.TaskItems.FindAsync(command.Id);
            if (existingItem == null)
                throw new ArgumentOutOfRangeException(nameof(command.Id));

            if (existingItem.Status == TaskItem.TaskStatus.Closed)
                throw new InvalidOperationException("cannot change task in this status");

            existingItem.ProjectID = command.ProjectId;
            existingItem.Priority = command.Priority;
            existingItem.Status = command.Status;
            existingItem.Description = command.Description;
            existingItem.Name = command.Name;
            existingItem.Modified = DateTime.UtcNow;

            _context.TaskItems.Add(existingItem);
            _context.Entry(existingItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return existingItem;
        }

        public async Task DeleteTask(int id)
        {
            var item = await _context.TaskItems.FindAsync(id);

            if (item == null)
                return;

            _context.TaskItems.Remove(item);

            await _context.SaveChangesAsync();
        }
    }
}
