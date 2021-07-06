using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskTracker.Database.Entities;
using TaskTracker.Database.Enums;

namespace TaskTracker.Services.Models
{
    public class TaskModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Priority { get; set; }
        public TaskStatus Status { get; set; }
        public int ProjectId { get; set; }
    }
}
