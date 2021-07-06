using FluentValidation;
using TaskTracker.Services.Models;

namespace TaskTracker.API.Validators
{
    public class TaskValidator : AbstractValidator<TaskModel>
    {
        public TaskValidator()
        {
            RuleFor(item => item.Status).IsInEnum();
            RuleFor(item => item.Priority).LessThan(6);
            RuleFor(item => item.ProjectId).GreaterThan(0);
        }
    }
}
