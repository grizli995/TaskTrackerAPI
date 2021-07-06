using System;
using System.Collections.Generic;
using TaskTracker.Database.Entities;
using TaskTracker.Database.Enums;
using TaskTracker.Services.Models;

namespace TaskTracker.Services
{
    /// <summary>
    /// Represents project service. Used for CRUD operations over Project entity.
    /// </summary>
    public interface IProjectService
    {
        /// <summary>
        /// Get array of projects based on filter parameters.
        /// </summary>
        /// <param name="filterName">Filter Name</param>
        /// <param name="filterPriority">Filter Priority</param>
        /// <param name="filterStatus">Filter Status</param>
        /// <param name="filterStartDate">Filter Start Date</param>
        /// <param name="filterEndDate">Filter End Date</param>
        /// <param name="sortBy">Sort By</param>
        /// <returns>Returns an array of projects.</returns>
        System.Threading.Tasks.Task<List<Project>> GetProjectsAsync(string filterName, int? filterPriority, ProjectStatus? filterStatus, DateTime? filterStartDate, DateTime? filterEndDate, ProjectSortingOrder? sortBy);

        /// <summary>
        /// Get project by identfier.
        /// </summary>
        /// <param name="id">Identfier</param>
        /// <returns>Returns project.</returns>
        System.Threading.Tasks.Task<Project> GetProjectAsync(int id);

        /// <summary>
        /// Add project.
        /// </summary>
        /// <param name="project">Project Model</param>
        /// <returns></returns>
        System.Threading.Tasks.Task AddProjectAsync(ProjectModel project);

        /// <summary>
        /// Update project.
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="newProject">Project Model</param>
        /// <returns></returns>
        System.Threading.Tasks.Task UpdateProjectAsync(int id, ProjectModel newProject);

        /// <summary>
        /// Delete project.
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns></returns>
        System.Threading.Tasks.Task DeleteProjectAsync(int id);
    }
}