using System;
using System.Collections.Generic;
using System.Linq;
using BugReport.Commands;
using BugReport.Models;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace BugReport.Repositories
{
    public class TaskRepository
    {
        private readonly BugTrackerContext _context;
        private readonly DbSet<TaskItem> _taskItemsList;

        public TaskRepository(BugTrackerContext context)
        {
            _context = context;
            _taskItemsList = context.TaskItems;
        }

        public List<TaskItem> GetTaskItems(string sortOrder,
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
            return items.ToPagedList(page, pageSize).ToList();
        }

        public TaskItem GetItem(int id)
        {
            return _context.TaskItems.Find(id);
        }

        public TaskItem CreateItem(CreateTaskItemCommand command)
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

            _context.SaveChanges();

            return taskItem;
        }

        public TaskItem UpdateItem(int id, UpdateTaskItemCommand command)
        {
            TaskItem existingItem = _context.TaskItems.Find(id);

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

            _context.SaveChanges();

            return existingItem;
        }

        public void DeleteItem(int id)
        {
            TaskItem item = _context.TaskItems.Find(id);

            _context.TaskItems.Remove(item);

            _context.SaveChanges();
        }

        /// <summary>
        /// Начальная инициализация элементов
        /// </summary>
        public void CreateInitialElementsIfNotExist()
        {
            if (_taskItemsList.Count() == 0)
            {
                _taskItemsList.Add(new TaskItem { Name = "Task 1", Description = "Task 1", Priority = TaskItem.TaskPriority.High, Created = DateTime.Now.AddDays(1), Modified = DateTime.UtcNow.AddDays(1) });
                _taskItemsList.Add(new TaskItem { Name = "Task 2", Description = "Task 2", Priority = TaskItem.TaskPriority.Low, Created = DateTime.Now.AddDays(-1), Modified = DateTime.UtcNow.AddDays(-1) });
                _taskItemsList.Add(new TaskItem { Name = "Task 3", Description = "Task 3", Priority = TaskItem.TaskPriority.Medium, Created = DateTime.Now.AddDays(2), Modified = DateTime.UtcNow.AddDays(2) });
                _context.SaveChanges();
            }
        }
    }
}
