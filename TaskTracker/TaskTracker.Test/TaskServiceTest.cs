using System;
using System.Collections.Generic;
using TaskTracker.Database.Entities;
using TaskTracker.Database.Enums;
using TaskTracker.Services;
using TaskTracker.Services.Models;
using Xunit;



namespace TaskTracker.Test
{
    public class TaskServiceTest
    {
        #region Fields

        private readonly TaskService _taskService;

        #endregion

        #region Constructor

        public TaskServiceTest()
        {
            _taskService = new TaskService(new Database.ApplicationDbContext(new Microsoft.EntityFrameworkCore.DbContextOptions<Database.ApplicationDbContext>()));
        }

        #endregion

        #region GetTasks

        [Theory]
        [InlineData("magna", null, null, null)]
        [InlineData("eu", null, null, TaskSortingOrder.PriorityDesc)]
        [InlineData(null, 1, null, null)]
        [InlineData(null, 1, null, TaskSortingOrder.StatusAsc)]
        [InlineData(null, null, null, null)]
        [InlineData(null, null, null, TaskSortingOrder.PriorityAsc)]
        public async System.Threading.Tasks.Task TestGetTasksWithoutDates(string filterName, int? filterPriority, TaskStatus? filterStatus, TaskSortingOrder? sortBy)
        {
            var tasks = await _taskService.GetTasksAsync(filterName, filterPriority, filterStatus, sortBy);

            Assert.NotEmpty(tasks);
        }


        [Fact]
        public async System.Threading.Tasks.Task TestGetTasksWithWrongName()
        {
            var name = "this doesnt exit";
            var tasks = await _taskService.GetTasksAsync(name, null, null, null);

            Assert.Empty(tasks);
        }

        #endregion

        #region GetTask


        [Theory]
        [InlineData(5)]
        public async System.Threading.Tasks.Task TestGetTaskValidId(int id)
        {
            var result = await _taskService.GetTaskAsync(id);

            Assert.NotNull(result);
        }

        [Theory]
        [InlineData(0)]
        public async System.Threading.Tasks.Task TestGetTaskIdZeroException(int id)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.GetTaskAsync(id));
        }

        [Theory]
        [InlineData(124121)]
        [InlineData(5454)]
        [InlineData(9342)]
        public async System.Threading.Tasks.Task TestGetTaskNonExistantId(int id)
        {
            var result = await _taskService.GetTaskAsync(id);

            Assert.Null(result);
        }

        #endregion

        #region AddTask

        [Fact]
        public async System.Threading.Tasks.Task TestAddTask()
        {
            var newTask = new TaskModel
            {
                Name = "This is a Test Task-" + DateTime.UtcNow.ToString(),
                Priority = 1,
                Status = TaskStatus.InProgress,
                ProjectId = 11
            };

            await _taskService.AddTaskAsync(newTask);
        }


        [Fact]
        public async System.Threading.Tasks.Task TestAddTaskNull()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(() => _taskService.AddTaskAsync(null));
        }

        #endregion

        #region UpdateTask

        [Fact]
        public async System.Threading.Tasks.Task TestUpdateTask()
        {
            var id = 5;
            var newTask = new TaskModel
            {
                Name = "UPDATED Test-" + DateTime.UtcNow.ToString(),
                Priority = 1,
                Status = TaskStatus.InProgress,
                ProjectId = 11
            };

            await _taskService.UpdateTaskAsync(id, newTask);

        }

        [Fact]
        public async System.Threading.Tasks.Task TestUpdateTaskNull()
        {
            var id = 5;

            await Assert.ThrowsAsync<ArgumentNullException>(() => _taskService.UpdateTaskAsync(id, null));
        }

        [Fact]
        public async System.Threading.Tasks.Task TestUpdateTaskIdZero()
        {
            var id = 0;

            var newTask = new TaskModel
            {
                Name = "UPDATED Test-" + DateTime.UtcNow.ToString(),
                Priority = 1,
                Status = TaskStatus.InProgress,
                ProjectId = 11
            };

            await Assert.ThrowsAsync<ArgumentException>(() => _taskService.UpdateTaskAsync(id, newTask));
        }

        [Fact]
        public async System.Threading.Tasks.Task TestUpdateTaskInvalidId()
        {
            var id = 41243;

            var newTask = new TaskModel
            {
                Name = "UPDATED Test-" + DateTime.UtcNow.ToString(),
                Priority = 1,
                Status = TaskStatus.InProgress,
                ProjectId = 11
            };

            await Assert.ThrowsAnyAsync<Exception>(() => _taskService.UpdateTaskAsync(id, newTask));
        }

        #endregion

        #region DeleteTask

        [Theory]
        [InlineData(1)]
        public async System.Threading.Tasks.Task TestDeleteTask(int id)
        {
            await _taskService.DeleteTaskAsync(id);
        }

        [Theory]
        [InlineData(412)]
        [InlineData(983)]
        [InlineData(81224)]
        public async System.Threading.Tasks.Task TestDeleteTaskInvalidId(int id)
        {
            await Assert.ThrowsAnyAsync<Exception>(() => _taskService.DeleteTaskAsync(id));
        }

        #endregion

        #region ChangeProjectId

        [Theory]
        [InlineData(1, 10)]
        [InlineData(2, 11)]
        [InlineData(3, 12)]
        public async System.Threading.Tasks.Task TestChangeProjectId(int taskId, int projectId)
        {
            await Assert.ThrowsAnyAsync<Exception>(() => _taskService.ChangeProjectAsync(taskId, projectId));
        }


        [Theory]
        [InlineData(0, 999)]
        [InlineData(983, 0)]
        public async System.Threading.Tasks.Task TestChangeProjectIdZeroId(int taskId, int projectId)
        {
            await Assert.ThrowsAnyAsync<ArgumentException>(() => _taskService.ChangeProjectAsync(taskId, projectId));
        }


        [Theory]
        [InlineData(412,999)]
        [InlineData(983,9123)]
        [InlineData(81224,91241)]
        public async System.Threading.Tasks.Task TestChangeProjectIdIdNotFound(int taskId, int projectId)
        {
            await Assert.ThrowsAnyAsync<InvalidOperationException>(() => _taskService.ChangeProjectAsync(taskId, projectId));
        }

        #endregion
    }
}
