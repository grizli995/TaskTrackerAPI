using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
    public class ProjectController : ControllerBase
    {
        #region Fields

        private readonly IProjectService _projectService;
        private readonly ILogger<Project> _logger;

        #endregion

        #region Constructors

        public ProjectController(ILogger<Project> logger, IProjectService projectService)
        {
            _logger = logger;
            _projectService = projectService;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns list of project entities based on filter parameters.
        /// </summary>
        /// <param name="filterName">Filter Name</param>
        /// <param name="filterPriority">Filter Priority</param>
        /// <param name="filterStatus">Filter Status</param>
        /// <param name="filterStartDate">Filter Start Date</param>
        /// <param name="filterEndDate">Filter End Date</param>
        /// <param name="sortBy">Sort By</param>
        /// <returns>List of Project Entities</returns>
        [HttpGet]
        [Route("/projects", Name = "QueryProjects")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Get(string filterName, int? filterPriority, ProjectStatus? filterStatus, DateTime? filterStartDate, DateTime? filterEndDate, ProjectSortingOrder? sortBy)
        {
            try
            {
                var projects = await _projectService.GetProjectsAsync(filterName, filterPriority, filterStatus, filterStartDate, filterEndDate, sortBy);
                var response = new JsonResult(projects);
                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetProjects failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }

        /// <summary>
        /// Get Project by key
        /// </summary>
        /// <param name="key">Project Identifier</param>
        /// <returns>Project Entity</returns>
        [HttpGet]
        [Route("/projects/{key}", Name = "GetProjectByKey")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Get([FromRoute] int key)
        {
            try
            {
                var project = await _projectService.GetProjectAsync(key);
                if (project == null)
                    return new JsonResult(StatusCode((int)HttpStatusCode.NotFound, key));

                var response = new JsonResult(project);
                return response;
            }
            catch (ArgumentException ae)
            {
                _logger.LogError(ae, "GetProjectByKey failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.BadRequest, ae));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "GetProjectByKey failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }

        /// <summary>
        /// Add Project entity
        /// </summary>
        /// <param name="input">Input Project model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/projects", Name = "CreateProject")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Post([FromBody] ProjectModel input)
        {
            try
            {
                await _projectService.AddProjectAsync(input);
                return new JsonResult(Ok());
            }
            catch (ArgumentNullException ae)
            {
                _logger.LogError(ae, "CreateProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.BadRequest, ae));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "CreateProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }

        /// <summary>
        /// Update Project entity
        /// </summary>
        /// <param name="key">Project Identifier</param>
        /// <param name="input">Input Project Model</param>
        /// <returns></returns>
        [HttpPut]
        [Route("/projects/{key}", Name = "UpdateProject")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Put([FromRoute] int key, [FromBody] ProjectModel input)
        {
            try
            {
                await _projectService.UpdateProjectAsync(key, input);
                return new JsonResult(Ok());
            }
            catch (ArgumentException ae)
            {
                _logger.LogError(ae, "UpdateProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.BadRequest, ae));
            }
            catch (InvalidOperationException ioe)
            {
                _logger.LogError(ioe, "UpdateProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.NotFound, ioe));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "UpdateProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }

        /// <summary>
        /// Delete Project entity by key
        /// </summary>
        /// <param name="key">Project Identifier</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("/projects/{key}", Name = "DeleteProject")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async System.Threading.Tasks.Task<IActionResult> Delete([FromRoute] int key)
        {
            try
            {
                await _projectService.DeleteProjectAsync(key);
                return new JsonResult(Ok());
            }
            catch (ArgumentException ae)
            {
                _logger.LogError(ae, "DeleteProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.BadRequest, ae));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "DeleteProject failed.");
                return new JsonResult(StatusCode((int)HttpStatusCode.InternalServerError, e));
            }
        }

        #endregion
    }
}
