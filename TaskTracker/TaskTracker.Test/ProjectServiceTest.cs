using System;
using System.Collections.Generic;
using TaskTracker.Database.Entities;
using TaskTracker.Database.Enums;
using TaskTracker.Services;
using TaskTracker.Services.Models;
using Xunit;

namespace TaskTracker.Test
{
    public class ProjectServiceTest
    {
        #region Fields

        private readonly ProjectService _projectService;

        #endregion

        #region Constructor

        public ProjectServiceTest()
        {
            _projectService = new ProjectService(new Database.ApplicationDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Database.ApplicationDbContext>()));
        }

        #endregion

        #region GetProjects

        [Theory]
        [InlineData("Aliquam", null, null, null, null, null)]
        [InlineData("elementum", null, null, null, null, ProjectSortingOrder.PriorityDesc)]
        [InlineData("facilisis", null, ProjectStatus.NotStarted, null, null, ProjectSortingOrder.PriorityDesc)]
        [InlineData(null, 1, null, null, null, null)]
        [InlineData(null, 1, null, null, null, ProjectSortingOrder.StatusAsc)]
        [InlineData(null, null, ProjectStatus.Active, null, null, null)]
        [InlineData(null, null, ProjectStatus.NotStarted, null, null, ProjectSortingOrder.PriorityAsc)]
        [InlineData("Suspendisse", 4, ProjectStatus.NotStarted, null, null, null)]
        public async System.Threading.Tasks.Task TestGetProjectsWithoutDates(string filterName, int? filterPriority, ProjectStatus? filterStatus, DateTime? filterStartDate, DateTime? filterEndDate, ProjectSortingOrder? sortBy)
        {
            var projects = await _projectService.GetProjectsAsync(filterName, filterPriority, filterStatus, filterStartDate, filterEndDate, sortBy);

            Assert.NotEmpty(projects);
        }

        [Fact]
        public async System.Threading.Tasks.Task TestGetProjectsWithStartDate()
        {
            var startDate = Convert.ToDateTime("12/28/20");
            var projects = await _projectService.GetProjectsAsync(null, null, null, startDate, null, null);

            Assert.NotEmpty(projects);
        }

        [Fact]
        public async System.Threading.Tasks.Task TestGetProjectsWithCompleteDate()
        {
            var endDate = Convert.ToDateTime("04/09/22");
            var projects = await _projectService.GetProjectsAsync(null, null, null, null, endDate, null);

            Assert.NotEmpty(projects);
        }


        [Fact]
        public async System.Threading.Tasks.Task TestGetProjectsWithWrongDate()
        {
            var endDate = Convert.ToDateTime("01/01/01");
            var projects = await _projectService.GetProjectsAsync(null, null, null, null, endDate, null);

            Assert.Empty(projects);
        }


        [Fact]
        public async System.Threading.Tasks.Task TestGetProjectsWithWrongName()
        {
            var name = "this doesnt exit";
            var projects = await _projectService.GetProjectsAsync(name, null, null, null, null, null);

            Assert.Empty(projects);
        }

        #endregion

        #region GetProject


        [Theory]
        [InlineData(5)]
        public async System.Threading.Tasks.Task TestGetProjectValidId(int id)
        {
            var result = _projectService.GetProjectAsync(id);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(0)]
        public async System.Threading.Tasks.Task TestGetProjectIdZeroException(int id)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _projectService.GetProjectAsync(id));
        }

        [Theory]
        [InlineData(124121)]
        [InlineData(5454)]
        [InlineData(9342)]
        public async System.Threading.Tasks.Task TestGetProjectNonExistantId(int id)
        {
            var result = await _projectService.GetProjectAsync(id);

            Assert.Null(result);
        }

        #endregion

        #region AddProject

        [Fact]
        public async System.Threading.Tasks.Task TestAddProject()
        {
            var newProject = new ProjectModel
            {
                Name = "This is a Test Project-" + DateTime.UtcNow.ToString(),
                StartDate = DateTime.UtcNow,
                CompleteDate = DateTime.UtcNow.AddMonths(3),
                Priority = 1,
                Status = ProjectStatus.Active,
                Tasks = new List<Task>()
            };

            await _projectService.AddProjectAsync(newProject);
        }


        [Fact]
        public async System.Threading.Tasks.Task TestAddProjectNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _projectService.AddProjectAsync(null));
        }

        #endregion

        #region UpdateProject

        [Fact]
        public async System.Threading.Tasks.Task TestUpdateProject()
        {
            var id = 5;
            var newProject = new ProjectModel
            {
                Name = "UPDATED Test-" + DateTime.UtcNow.ToString(),
                StartDate = DateTime.UtcNow,
                CompleteDate = DateTime.UtcNow.AddMonths(3),
                Priority = 1,
                Status = ProjectStatus.Active,
                Tasks = new List<Task>()
            };

            await _projectService.UpdateProjectAsync(id, newProject);

        }

        [Fact]
        public async System.Threading.Tasks.Task TestUpdateProjectNull()
        {
            var id = 5;

            await Assert.ThrowsAsync<ArgumentNullException>(() => _projectService.UpdateProjectAsync(id, null));
        }

        [Fact]
        public async System.Threading.Tasks.Task TestUpdateProjectIdZero()
        {
            var id = 0;

            var newProject = new ProjectModel
            {
                Name = "UPDATED Test-" + DateTime.UtcNow.ToString(),
                StartDate = DateTime.UtcNow,
                CompleteDate = DateTime.UtcNow.AddMonths(3),
                Priority = 1,
                Status = ProjectStatus.Active,
                Tasks = new List<Task>()
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _projectService.UpdateProjectAsync(id, newProject));
        }

        [Fact]
        public async System.Threading.Tasks.Task TestUpdateProjectInvalidId()
        {
            var id = 41243;

            var newProject = new ProjectModel
            {
                Name = "UPDATED Test-" + DateTime.UtcNow.ToString(),
                StartDate = DateTime.UtcNow,
                CompleteDate = DateTime.UtcNow.AddMonths(3),
                Priority = 1,
                Status = ProjectStatus.Active,
                Tasks = new List<Task>()
            };

            await Assert.ThrowsAnyAsync<Exception>(() => _projectService.UpdateProjectAsync(id, newProject));
        }

        #endregion

        #region DeleteProject

        [Theory]
        [InlineData(1)]
        public async System.Threading.Tasks.Task TestDeleteProject(int id)
        {
            await _projectService.DeleteProjectAsync(id);
        }

        [Theory]
        [InlineData(412)]
        [InlineData(983)]
        [InlineData(81224)]
        public async System.Threading.Tasks.Task TestDeleteProjectInvalidId(int id)
        {
            await Assert.ThrowsAnyAsync<Exception>(() =>  _projectService.DeleteProjectAsync(id));
        }

        #endregion
    }
}
