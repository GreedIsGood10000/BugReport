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
    public class ProjectController : ControllerBase
    {
        private const int DEFAULT_PAGE_SIZE = 30;
        private const int DEFAULT_PAGE = 1;
        private const int MIN_PAGE = 0;
        private const int MAX_PAGE = int.MaxValue;
        private const int MIN_PAGE_SIZE = 1;
        private const int MAX_PAGE_SIZE = 100;

        private readonly IProjectRepository _projectRepository;

        public ProjectController(IProjectRepository projectProjectRepository)
        {
            _projectRepository = projectProjectRepository;
        }

        /// <summary>
        /// Чтение всех записей
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectItem>>> ReadItems(
            [FromQuery(Name = "page")] [Range(MIN_PAGE, MAX_PAGE)] int page = DEFAULT_PAGE,
            [FromQuery(Name = "pagesize")][Range(MIN_PAGE_SIZE, MAX_PAGE_SIZE)] int pageSize = DEFAULT_PAGE_SIZE)
        {
            try
            {
                var readTasksCommand = new ReadTasksCommand
                {
                    Page = page,
                    PageSize = pageSize
                };

                var item = await _projectRepository.GetProjectItems(readTasksCommand);

                return Ok(item);
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
        public async Task<ActionResult<ProjectItem>> ReadItem([FromRoute] int id)
        {
            try
            {
                ProjectItem result = await _projectRepository.GetItem(id);

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
        public async Task<ActionResult<ProjectItem>> CreateItem([FromBody] CreateProjectParameters parameters)
        {
            try
            {
                var command = new CreateProjectItemCommand
                {
                    Name = parameters.Name,
                    Description = parameters.Description
                };

                ProjectItem projectItem = await _projectRepository.CreateItem(command);

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
        /// <param name="parameters"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProjectItem>> UpdateItem(
            [FromRoute(Name = "id")] [Range(0, int.MaxValue)] int id,
            [FromBody] UpdateProjectParameters parameters)
        {
            try
            {
                var command = new UpdateProjectItemCommand
                {
                    Id = id,
                    Name = parameters.Name,
                    Description = parameters.Description
                };

                return await _projectRepository.UpdateItem(command);
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
        public async Task<ActionResult> RemoveItem([FromRoute] int id)
        {
            try
            {
                await _projectRepository.DeleteItem(id);

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