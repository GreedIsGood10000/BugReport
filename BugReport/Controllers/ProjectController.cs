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

            _repository.CreateInitialElementsIfNotExist();
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProjectItem>> ReadItems(
            [FromQuery] int page = DefaultPageNumber,
            [FromQuery] [Range(MinPageSize, MaxPageSize)] int pageSize = DefaultPageSize)
        {
            try
            {
                return _repository.GetProjectItems(page, pageSize);
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ProjectItem> ReadItem(int id)
        {
            try
            {
                ProjectItem result = _repository.GetItem(id);

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
        public ActionResult<ProjectItem> CreateItem(CreateProjectItemCommand command)
        {
            try
            {
                ProjectItem projectItem = _repository.CreateItem(command);

                return CreatedAtAction(nameof(ReadItem), new {id = projectItem.ID}, projectItem);
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateItem(int id, UpdateProjectItemCommand item)
        {
            try
            {
                _repository.UpdateItem(id, item);
            }
            catch (Exception e)
            {
                //Logger.Log(e);
                return BadRequest();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveItem(int id)
        {
            try
            {
                ProjectItem item = _repository.GetItem(id);

                if (item == null)
                    //todo посмотреть, в каком виде можно вернуть ошибку
                    return NotFound();

                _repository.DeleteItem(id);

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