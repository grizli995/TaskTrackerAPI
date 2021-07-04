using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskTracker.Database.Entities;
using TaskTracker.Services;

namespace TaskTracker.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        [HttpGet]
        [Route("/projects", Name = "QueryProjects")]
        public IActionResult Get()
        {
            var x = _projectService.GetProject(0);
            var response = new JsonResult(x);
            return response;
        }

        [HttpGet]
        [Route("/projects/{key}", Name = "GetProjectByKey")]
        public IActionResult Get([FromRoute] int key)
        {
            var x = _projectService.GetProject(0);
            var response = new JsonResult(x);
            return response;
        }

        [HttpPost]
        [Route("/projects", Name = "CreateProject")]
        public IActionResult Post([FromBody]Project input)
        {
            var x = _projectService.GetProject(0);
            var response = new JsonResult(x);
            return response;
        }

        [HttpPut]
        [Route("/projects/{key}", Name = "UpdateProject")]
        public IActionResult Put([FromRoute] int key, [FromBody] Project input)
        {
            var x = _projectService.GetProject(0);
            var response = new JsonResult(x);
            return response;
        }

        [HttpDelete]
        [Route("/projects/{key}", Name = "DeleteProject")]
        public IActionResult Delete([FromRoute] int key)
        {
            var x = _projectService.GetProject(0);
            var response = new JsonResult(x);
            return response;
        }

        #endregion
    }
}
