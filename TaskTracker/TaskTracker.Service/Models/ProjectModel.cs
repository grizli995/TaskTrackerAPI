using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskTracker.Database.Entities;
using TaskTracker.Database.Enums;

namespace TaskTracker.Services.Models
{
    public class ProjectModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime CompleteDate { get; set; }
        public int Priority { get; set; }
        public ProjectStatus Status { get; set; }
        public List<Task> Tasks { get; set; }
    }
}
