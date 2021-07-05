using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskTracker.Database;
using TaskTracker.Database.Entities;
using TaskTracker.Database.Enums;
using TaskTracker.Services.Models;

namespace TaskTracker.Services
{
    /// <summary>
    /// Represents project service. Used for CRUD operations over Project entity.
    /// </summary>
    public class ProjectService : IProjectService
    {
        private const string ValueGreaterThanZeroMessage = "Invalid argument exception. Id value must be greater than 0.";

        #region Fields

        private readonly ApplicationDbContext _context;

        #endregion

        #region Constructors

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        #endregion

        #region Methods

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
        public async System.Threading.Tasks.Task<List<Project>> GetProjectsAsync(string filterName, int? filterPriority, ProjectStatus? filterStatus, DateTime? filterStartDate, DateTime? filterEndDate, SortingOrder? sortBy)
        {
            var query = BuildQueryable(filterName, filterPriority, filterStatus, filterStartDate, filterEndDate);

            var orderedQuery = AddSorting(query, sortBy);

            return await orderedQuery.ToListAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get project by identfier.
        /// </summary>
        /// <param name="id">Identfier</param>
        /// <returns>Returns project.</returns>
        public async System.Threading.Tasks.Task<Project> GetProjectAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(ValueGreaterThanZeroMessage);

            var result = await _context.Projects.FindAsync(id).ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Add project.
        /// </summary>
        /// <param name="project">Project Model</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task AddProjectAsync(ProjectModel project)
        {
            if (project == null)
                throw new ArgumentNullException($"Argument '{nameof(project)}' is null.");

            _context.Projects.Add(ConvertModelToEntity(project));

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Update project.
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <param name="newProject">Project Model</param>
        /// <returns></returns>
        public async System.Threading.Tasks.Task UpdateProjectAsync(int id, ProjectModel newProject)
        {
            if (id <= 0)
                throw new ArgumentException(ValueGreaterThanZeroMessage);

            if (newProject == null)
                throw new ArgumentNullException($"Argument '{nameof(newProject)}' is null.");

            var project = _context.Projects.First(item => item.Id == id);

            project.StartDate = newProject.StartDate;
            project.CompleteDate = newProject.CompleteDate;
            project.Status = newProject.Status;
            project.Priority = newProject.Priority;
            project.Name = newProject.Name;

            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Delete project.
        /// </summary>
        /// <param name="id">Identifier</param>
        public async System.Threading.Tasks.Task DeleteProjectAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(ValueGreaterThanZeroMessage);

            _context.Projects.Remove(_context.Projects.First(item => item.Id == id));
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion

        #region Util

        /// <summary>
        /// Convert input ProjectModel to Project Entity.
        /// </summary>
        /// <param name="model">Input model</param>
        /// <returns>Project Entity</returns>
        private Project ConvertModelToEntity(ProjectModel model)
        {
            return new Project
            {
                Name = model.Name,
                Priority = model.Priority,
                Status = model.Status,
                Tasks = model.Tasks,
                StartDate = model.StartDate,
                CompleteDate = model.CompleteDate
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
        /// <returns>IQueryable<Project> based on filter paramteres.</returns>
        private IQueryable<Project> BuildQueryable(string filterName, int? filterPriority, ProjectStatus? filterStatus, DateTime? filterStartDate, DateTime? filterEndDate)
        {
            var query = _context.Projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterName))
                query = query.Where(item => item.Name.Contains(filterName));

            if (filterPriority.HasValue && filterPriority.Value > 0)
                query = query.Where(item => item.Priority == filterPriority);

            if (filterStatus.HasValue)
                query = query.Where(item => item.Status == filterStatus);

            if (filterStartDate.HasValue)
                query = query.Where(item => item.StartDate == filterStartDate);

            if (filterEndDate.HasValue)
                query = query.Where(item => item.CompleteDate == filterEndDate);

            return query;
        }

        /// <summary>
        /// Add sorting to the query based on sortBy paramter.
        /// </summary>
        /// <param name="query">Input query</param>
        /// <param name="sortBy">Sort By</param>
        /// <returns>IOrderedQueryable<Project> based on sortBy parameter.</returns>
        private IOrderedQueryable<Project> AddSorting(IQueryable<Project> query, SortingOrder? sortBy)
        {
            if (!sortBy.HasValue)
                return query.OrderBy(item => item.Id);

            switch (sortBy.Value)
            {
                case (SortingOrder.NameAsc):
                    return query.OrderBy(item => item.Name);
                case (SortingOrder.NameDesc):
                    return query.OrderByDescending(item => item.Name);
                case (SortingOrder.StartDateAsc):
                    return query.OrderBy(item => item.StartDate);
                case (SortingOrder.StartDateDesc):
                    return query.OrderByDescending(item => item.StartDate);
                case (SortingOrder.CompleteDateAsc):
                    return query.OrderBy(item => item.CompleteDate);
                case (SortingOrder.CompleteDateDesc):
                    return query.OrderByDescending(item => item.CompleteDate);
                case (SortingOrder.PriorityAsc):
                    return query.OrderBy(item => item.Priority);
                case (SortingOrder.PriorityDesc):
                    return query.OrderByDescending(item => item.Priority);
                case (SortingOrder.StatusAsc):
                    return query.OrderBy(item => item.Status);
                case (SortingOrder.StatusDesc):
                    return query.OrderByDescending(item => item.Status);
                default:
                    return query.OrderBy(item => item.Id);
            }
        }

        #endregion
    }
}
