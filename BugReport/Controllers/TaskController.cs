using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BugReport.Commands;
using BugReport.Models;
using BugReport.Parameters;
using BugReport.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BugReport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private const int DEFAULT_PAGE_SIZE = 30;
        private const int DEFAULT_PDAGE_NUMBER = 1;
        private const int MIN_PAGE_SIZE = 1;
        private const int MAX_PAGE_SIZE = 100;

        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        /// <summary>
        /// Чтение записей с фильтрацией
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="projectId"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="status"></param>
        /// <param name="priority"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItem>>> ReadItems(
            [FromQuery(Name = "sortorder")] string sortOrder, 
            [FromQuery(Name = "projectid")] int[] projectId,
            [FromQuery(Name = "datefrom")] DateTime? dateFrom,
            [FromQuery(Name = "dateto")] DateTime? dateTo,
            [FromQuery(Name = "status")] TaskItem.TaskStatus[] status,
            [FromQuery(Name = "priority")] TaskItem.TaskPriority[] priority,
            [FromQuery(Name = "page")] int page = DEFAULT_PDAGE_NUMBER,
            [FromQuery(Name = "pagesize")] [Range(MIN_PAGE_SIZE, MAX_PAGE_SIZE)] int pageSize = DEFAULT_PAGE_SIZE
            )
        {
            try
            {
                return await _taskRepository.GetTaskItems(sortOrder, projectId, dateFrom, dateTo, status, priority, page,
                pageSize);
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Чтение записи по ид
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> ReadItem(int id)
        {
            try
            {
                TaskItem result = await _taskRepository.GetItem(id);

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

        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateItem([FromBody] CreateTaskParameters parameters)
        {
            try
            {
                var command = new CreateTaskItemCommand
                {
                    Name = parameters.Name,
                    Description = parameters.Description,
                    Priority = parameters.Priority,
                    ProjectID = parameters.ProjectId
                };

                return await _taskRepository.CreateItem(command);
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Обновление записи
        /// </summary>
        /// <param name="id"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskItem>> UpdateItem(
            [FromRoute] int id,
            [FromBody] UpdateTaskParameters parameters)
        {
            try
            {
                var command = new UpdateTaskItemCommand
                {  
                    Id = id,
                    Name = parameters.Name,
                    Description = parameters.Description,
                    Priority = parameters.Priority,
                    ProjectId = parameters.ProjectId,
                    Status = parameters.Status
                };

                return await _taskRepository.UpdateItem(command);
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }

        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            try
            {
                await _taskRepository.DeleteItem(id);

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