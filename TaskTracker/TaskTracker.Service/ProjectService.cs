using System;
using System.Collections.Generic;
using System.Linq;
using TaskTracker.Database;
using TaskTracker.Database.Entities;

namespace TaskTracker.Services
{
    public class ProjectService : IProjectService
    {
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

        public List<Project> GetProjects()
        {
            return new List<Project>();
        }
        public Project GetProject(int id)
        {
            var result =  _context.Projects.Where(item => item.Id == id).First();
            return result;
        }
        public bool AddProject(Project project)
        {
            _context.Projects.Add(project);
            return true;
        }
        public bool UpdateProject(int id, Project newProject)
        {
            var project = _context.Projects.First(item => item.Id == id);
            project = newProject;
            return true;
        }
        public bool DeleteProject(int id)
        {
            _context.Projects.Remove(_context.Projects.First(item => item.Id == id));
            return true;
        }

        #endregion

    }
}
