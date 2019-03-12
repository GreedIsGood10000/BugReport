using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BugReport.Commands;
using BugReport.Models;
using BugReport.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BugReport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private const int DefaultPageSize = 30;
        private const int DefaultPageNumber = 1;
        private const int MinPageSize = 1;
        private const int MaxPageSize = 100;

        private readonly TaskRepository _taskRepository;

        public TaskController(BugTrackerContext context)
        {
            _taskRepository = new TaskRepository(context);

            _taskRepository.CreateInitialElementsIfNotExist();
        }

        [HttpGet]
        public ActionResult<IEnumerable<TaskItem>> ReadItems(
            [FromQuery] string sortOrder, 
            [FromQuery] int[] projectId,
            [FromQuery] DateTime? dateFrom,
            [FromQuery] DateTime? dateTo,
            [FromQuery] TaskItem.TaskStatus[] status,
            [FromQuery] TaskItem.TaskPriority[] priority,
            [FromQuery] int page = DefaultPageNumber,
            [FromQuery] [Range(MinPageSize, MaxPageSize)] int pageSize = DefaultPageSize
            )
        {
            try
            {
                return _taskRepository.GetTaskItems(sortOrder, projectId, dateFrom, dateTo, status, priority, page,
                pageSize);
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<TaskItem> ReadItem(int id)
        {
            try
            {
                TaskItem result = _taskRepository.GetItem(id);

                if (result == null)
                    return NotFound();

                return result;
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }

        [HttpPost]
        public ActionResult<TaskItem> CreateItem(CreateTaskItemCommand item)
        {
            try
            {
                TaskItem taskItem = _taskRepository.CreateItem(item);

                //todo получается, метод будет стучаться до readItem?? проверить на отладчике
                //todo почему подчеркивает nameof(ReadItem)?
                return CreatedAtAction(nameof(ReadItem), new { id = taskItem.ID }, taskItem);
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }

        //todo откуда метод берёт TaskItem? попробовать подставить этот параметр в другой метод
        [HttpPut("{id}")]
        public ActionResult UpdateItem(int id, UpdateTaskItemCommand item)
        {
            try
            {
                _taskRepository.UpdateItem(id, item);

                //todo узнать, получается, коды ответа в бразуер возращаются либо стандартыне при возврате значения, либо кастомные с помощью методов?
                return NoContent();
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteItem(int id)
        {
            try
            {
                TaskItem item = _taskRepository.GetItem(id);

                if (item == null)
                    return NotFound();

                _taskRepository.DeleteItem(id);

                return NoContent();
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }
    }
}