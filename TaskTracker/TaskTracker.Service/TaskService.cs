using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskTracker.Database;
using TaskTracker.Database.Entities;
using TaskTracker.Database.Enums;
using TaskTracker.Services.Models;

namespace TaskTracker.Services
{
    /// <summary>
    /// Represents task service. Used for CRUD operations over Task entity.
    /// </summary>
    public class TaskService : ITaskService
    {
        private const string ValueGreaterThanZeroMessage = "Invalid argument exception. Id value must be greater than 0.";

        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructors

        public TaskService(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get array of task based on filter parameters.
        /// </summary>
        /// <param name="filterName">Filter Name</param>
        /// <param name="filterPriority">Filter Priority</param>
        /// <param name="filterStatus">Filter Status</param>
        /// <param name="sortBy">Sort By</param>
        /// <returns>Returns an array of tasks.</returns>
        public async System.Threading.Tasks.Task<List<Task>> GetTasksAsync(string filterName, int? filterPriority, TaskStatus? filterStatus, TaskSortingOrder? sortBy)
        {
            var query = BuildQueryable(filterName, filterPriority, filterStatus);

            var orderedQuery = AddSorting(query, sortBy);

            return await orderedQuery.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get task by identfier.
        /// </summary>
        /// <param name="id">Identfier</param>
        /// <returns>Returns task.</returns>
        public async System.Threading.Tasks.Task<Task> GetTaskAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(ValueGreaterThanZeroMessage);

            var result = await _context.Tasks.FindAsync(id).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Add task.
        /// </summary>
        /// <param name="task">Task Model</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task AddTaskAsync(TaskModel task)
        {
            if (task == null)
                throw new ArgumentNullException($"Argument '{nameof(task)}' is null.");

            _context.Tasks.Add(ConvertModelToEntity(task));

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }


        /// <summary>
        /// Update task.
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="newTask">Task Model</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task UpdateTaskAsync(int id, TaskModel newTask)
        {
            if (id <= 0)
                throw new ArgumentException(ValueGreaterThanZeroMessage);
            if (newTask == null)
                throw new ArgumentNullException($"Argument '{nameof(newTask)}' is null.");

            var task = _context.Tasks.First(item => item.Id == id);

            task.Status = newTask.Status;
            task.Priority = newTask.Priority;
            task.Name = newTask.Name;
            task.Description = newTask.Description;
            task.ProjectId = newTask.ProjectId;

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Delete task.
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task DeleteTaskAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(ValueGreaterThanZeroMessage);
             _context.Tasks.Remove(_context.Tasks.First(item => item.Id == id));
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Change project identifier for the task with the provided task identifier.
        /// </summary>
        /// <param name="taskId">Task Identifier</param>
        /// <param name="projectId">Project Identifier</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task ChangeProjectAsync(int taskId, int projectId)
        {
            if (taskId <= 0 || projectId <= 0)
                throw new ArgumentException(ValueGreaterThanZeroMessage);

            var task = _context.Tasks.FindAsync(taskId).AsTask();
            var projectExists = _context.Projects.AnyAsync(item => item.Id == projectId);

            await System.Threading.Tasks.Task.WhenAll(task, projectExists).ConfigureAwait(false);

            var taskToUpdate = task.Result;
            if (taskToUpdate == null || projectExists.Result == false)
                throw new InvalidOperationException();

            taskToUpdate.ProjectId = projectId;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion

        #region Util

        /// <summary>
        /// Convert input TaskModel to Task Entity.
        /// </summary>
        /// <param name="model">Input model</param>
        /// <returns>Task Entity</returns>
        private Task ConvertModelToEntity(TaskModel model)
        {
            return new Task
            {
                Name = model.Name,
                Description = model.Description,
                Status = model.Status,
                Priority = model.Priority,
                ProjectId = model.ProjectId
            };
        }

        /// <summary>
        /// Create query based on filter parameters.
        /// </summary>
        /// <param name="filterName">Filter Name</param>
        /// <param name="filterPriority">Filter Priority</param>
        /// <param name="filterStatus">Filter Status</param>
        /// <param name="filterStartDate">Filter Start Date</param>
        /// <param name="filterEndDate">Filter End Date</param>
        /// <returns>IQueryable<Task> based on filter paramteres.</returns>
        private IQueryable<Task> BuildQueryable(string filterName, int? filterPriority, TaskStatus? filterStatus)
        {
            var query = _context.Tasks.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterName))
                query = query.Where(item => item.Name.Contains(filterName));

            if (filterPriority.HasValue && filterPriority.Value > 0)
                query = query.Where(item => item.Priority == filterPriority);

            if (filterStatus.HasValue)
                query = query.Where(item => item.Status == filterStatus.Value);

            return query;
        }

        /// <summary>
        /// Add sorting to the query based on sortBy paramter.
        /// </summary>
        /// <param name="query">Input query</param>
        /// <param name="sortBy">Sort By</param>
        /// <returns>IOrderedQueryable<Task> based on sortBy parameter.</returns>
        private IOrderedQueryable<Task> AddSorting(IQueryable<Task> query, TaskSortingOrder? sortBy)
        {
            if (!sortBy.HasValue)
                return query.OrderBy(item => item.Id);

            switch (sortBy.Value)
            {
                case (TaskSortingOrder.NameAsc):
                    return query.OrderBy(item => item.Name);
                case (TaskSortingOrder.NameDesc):
                    return query.OrderByDescending(item => item.Name);
                case (TaskSortingOrder.PriorityAsc):
                    return query.OrderBy(item => item.Priority);
                case (TaskSortingOrder.PriorityDesc):
                    return query.OrderByDescending(item => item.Priority);
                case (TaskSortingOrder.StatusAsc):
                    return query.OrderBy(item => item.Status);
                case (TaskSortingOrder.StatusDesc):
                    return query.OrderByDescending(item => item.Status);
                default:
                    return query.OrderBy(item => item.Id);
            }
        }

        #endregion
    }
}
