using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using TaskTracker.Database.Entities;
using TaskTracker.Database.Enums;
using TaskTracker.Services;
using TaskTracker.Services.Models;

namespace TaskTracker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class TaskController : ControllerBase
    {
        #region Fields

        private readonly ITaskService _taskService;
        private readonly ILogger<Task> _logger;

        #endregion

        #region Constructors

        public TaskController(ILogger<Task> logger, ITaskService taskService)
        {
            _logger = logger;
            _taskService = taskService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns list of task entities based on filter parameters.
        /// </summary>
        /// <param name="filterName">Filter Name</param>
        /// <param name="filterPriority">Filter Priority</param>
        /// <param name="filterStatus">Filter Status</param>
        /// <param name="sortBy">Sort By</param>
        /// <returns>List of Task Entities</returns>
        [HttpGet]
        [Route("/tasks", Name = "QueryTasks")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Get(string filterName, int? filterPriority, TaskStatus? filterStatus, TaskSortingOrder? sortBy)
        {
            try
            {
                var tasks = await _taskService.GetTasksAsync(filterName, filterPriority, filterStatus, sortBy);
                var response = new JsonResult(tasks);
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "QueryTasks failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }

        /// <summary>
        /// Get Task by key
        /// </summary>
        /// <param name="key">Task Identifier</param>
        /// <returns>Task Entity</returns>
        [HttpGet]
        [Route("/tasks/{key}", Name = "GetTaskByKey")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Get([FromRoute] int key)
        {
            try
            {
                var task = await _taskService.GetTaskAsync(key);
                if (task == null)
                    return new JsonResult(StatusCode((int)HttpStatusCode.NotFound, key));

                var response = new JsonResult(task);
                return response;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError(ae, "GetTaskByKey failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.BadRequest, ae));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetTaskByKey failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }

        /// <summary>
        /// Add Task entity
        /// </summary>
        /// <param name="input">Input Task model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/tasks", Name = "CreateTask")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Post([FromBody] TaskModel input)
        {
            try
            {
                await _taskService.AddTaskAsync(input);
                return new JsonResult(Ok());
            }
            catch (ArgumentNullException ae)
            {
                _logger.LogError(ae, "CreateTask failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.BadRequest, ae));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CreateTask failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }

        /// <summary>
        /// Update Task entity
        /// </summary>
        /// <param name="key">Task Identifier</param>
        /// <param name="input">Input Task Model</param>
        /// <returns></returns>
        [HttpPut]
        [Route("/tasks/{key}", Name = "UpdateTask")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Put([FromRoute] int key, [FromBody] TaskModel input)
        {
            try
            {
                await _taskService.UpdateTaskAsync(key, input);
                return new JsonResult(Ok());
            }
            catch (ArgumentException ae)
            {
                _logger.LogError(ae, "UpdateTask failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.BadRequest, ae));
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError(ioe, "UpdateTask failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.NotFound, ioe));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdateTask failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }

        /// <summary>
        /// Delete Task entity by key
        /// </summary>
        /// <param name="key">Task Identifier</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("/tasks/{key}", Name = "DeleteTask")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Delete([FromRoute] int key)
        {
            try
            {
                await _taskService.DeleteTaskAsync(key);
                return new JsonResult(Ok());
            }
            catch (ArgumentException ae)
            {
                _logger.LogError(ae, "DeleteTask failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.BadRequest, ae));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeleteTask failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }



        /// <summary>
        /// Change project for Task entity by key
        /// </summary>
        /// <param name="taskKey">Task Identifier</param>
        /// <param name="projectKey">Task Identifier</param>
        /// <returns></returns>
        [HttpPatch]
        [Route("/tasks/{taskKey}", Name = "ChangeProject")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> ChangeProjectId([FromRoute] int taskKey, [FromQuery] int projectKey)
        {
            try
            {
                await _taskService.ChangeProjectAsync(taskKey, projectKey);
                return new JsonResult(Ok());
            }
            catch (ArgumentException ae)
            {
                _logger.LogError(ae, "ChangeProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.BadRequest, ae));
            }
            catch (InvalidOperationException ae)
            {
                _logger.LogError(ae, "ChangeProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.NotFound, ae));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ChangeProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }
        #endregion
    }
}
