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
        private readonly DbSet<TaskItem> _taskItemsList;

        public TaskRepository(BugTrackerContext context)
        {
            _context = context;
            _taskItemsList = context.TaskItems;
        }

        public async Task<List<TaskItem>> GetTaskItems(string sortOrder,
            int[] projectId,
            DateTime? dateFrom,
            DateTime? dateTo,
            TaskItem.TaskStatus[] status,
            TaskItem.TaskPriority[] priority,
            int page,
            int pageSize)
        {
            IEnumerable<TaskItem> items = _context.TaskItems;

            switch (sortOrder)
            {
                case "priority":
                    items = items.OrderBy(x => x.Priority);
                    break;
                case "created":
                    items = items.OrderBy(x => x.Created);
                    break;
            }

            if (projectId?.Length != 0)
            {
                items = items.Where(x => projectId.Contains(x.ProjectID));
            }

            if (status?.Length != 0)
            {
                items = items.Where(x => status.Contains(x.Status));
            }

            if (priority?.Length != 0)
            {
                items = items.Where(x => priority.Contains(x.Priority));
            }

            if (dateFrom != null)
            {
                items = items.Where(x => x.Created >= dateFrom);
            }

            if (dateTo != null)
            {
                items = items.Where(x => x.Created <= dateTo);
            }

            //используется пакет X.PagedList.Mvc.Core
            var itemsPagedList = await items.ToPagedListAsync(page, pageSize);

            return await itemsPagedList.ToListAsync();
        }

        public Task<TaskItem> GetItem(int id)
        {
            return _context.TaskItems.FindAsync(id);
        }

        public async Task<TaskItem> CreateItem(CreateTaskItemCommand command)
        {
            TaskItem taskItem = new TaskItem
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

        public async Task<TaskItem> UpdateItem(int id, UpdateTaskItemCommand command)
        {
            TaskItem existingItem = await _context.TaskItems.FindAsync(id);

            if (existingItem.Status == TaskItem.TaskStatus.Closed)
                throw new InvalidOperationException("cannot change task in this status");

            existingItem.ProjectID = command.ProjectID;
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

        public async Task DeleteItem(int id)
        {
            TaskItem item = await _context.TaskItems.FindAsync(id);

            if (item == null)
                return;

            _context.TaskItems.Remove(item);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Начальная инициализация элементов
        /// </summary>
        public void CreateInitialElementsIfNotExist()
        {
            if (!_taskItemsList.Any())
            {
                _context.SaveChanges();
            }
        }
    }
}
