using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using BugReport.Commands;
using BugReport.Models;
using BugReport.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BugReport.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private const int DefaultPageSize = 30;
        private const int DefaultPageNumber = 1;
        private const int MinPageSize = 1;
        private const int MaxPageSize = 100;

        private readonly ProjectRepository _repository;

        public ProjectController(BugTrackerContext context)
        {
            _repository = new ProjectRepository(context);

            //начальное заполнение данными
            _repository.CreateInitialElementsIfNotExist();
        }

        /// <summary>
        /// Чтение всех записей
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectItem>>> ReadItems(
            [FromQuery] int page = DefaultPageNumber,
            [FromQuery] [Range(MinPageSize, MaxPageSize)] int pageSize = DefaultPageSize)
        {
            try
            {
                var e = await _repository.GetProjectItems(page, pageSize);
                return Ok(e);
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
        public async Task<ActionResult<ProjectItem>> ReadItem(int id)
        {
            try
            {
                ProjectItem result = await _repository.GetItem(id);

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
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ProjectItem>> CreateItem(CreateProjectItemCommand command)
        {
            try
            {
                ProjectItem projectItem = await _repository.CreateItem(command);

                return projectItem;
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
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItem(int id, UpdateProjectItemCommand item)
        {
            try
            {
                await _repository.UpdateItem(id, item);
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }

            return NoContent();
        }


        /// <summary>
        /// Удаление записи
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveItem(int id)
        {
            try
            {
                await _repository.DeleteItem(id);

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