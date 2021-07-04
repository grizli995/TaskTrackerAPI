using TaskTracker.Database.Entities;

namespace TaskTracker.Services
{
    public interface IProjectService
    {
        Project GetProject(int id);
    }
}