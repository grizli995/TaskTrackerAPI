using System;
using System.Collections.Generic;
using TaskTracker.Database.Entities;
using TaskTracker.Database.Enums;
using TaskTracker.Services.Models;

namespace TaskTracker.Services
{
    /// <summary>
    /// Represents task service. Used for CRUD operations over Task entity.
    /// </summary>
    public interface ITaskService
    {
        /// <summary>
        /// Get array of task based on filter parameters.
        /// </summary>
        /// <param name="filterName">Filter Name</param>
        /// <param name="filterPriority">Filter Priority</param>
        /// <param name="filterStatus">Filter Status</param>
        /// <param name="sortBy">Sort By</param>
        /// <returns>Returns an array of tasks.</returns>
        System.Threading.Tasks.Task<List<Task>> GetTasksAsync(string filterName, int? filterPriority, TaskStatus? filterStatus , TaskSortingOrder? sortBy);

        /// <summary>
        /// Get task by identfier.
        /// </summary>
        /// <param name="id">Identfier</param>
        /// <returns>Returns task.</returns>
        System.Threading.Tasks.Task<Task> GetTaskAsync(int id);

        /// <summary>
        /// Add task.
        /// </summary>
        /// <param name="project">Task Model</param>
        /// <returns></returns>
        System.Threading.Tasks.Task AddTaskAsync(TaskModel project);

        /// <summary>
        /// Update task.
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="newProject">Task Model</param>
        /// <returns></returns>
        System.Threading.Tasks.Task UpdateTaskAsync(int id, TaskModel newProject);

        /// <summary>
        /// Delete task.
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns></returns>
        System.Threading.Tasks.Task DeleteTaskAsync(int id);

        /// <summary>
        /// Change project identifier for the task with the provided task identifier.
        /// </summary>
        /// <param name="taskId">Task Identifier</param>
        /// <param name="projectId">Project Identifier</param>
        /// <returns></returns>
        System.Threading.Tasks.Task ChangeProjectAsync(int taskId, int projectId);
    }
}