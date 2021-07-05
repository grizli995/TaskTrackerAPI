using FluentValidation;
using TaskTracker.Services.Models;

namespace TaskTracker.API.Validators
{
    public class ProjectValidator : AbstractValidator<ProjectModel>
    {
        public ProjectValidator()
        {
            RuleFor(item => item.Status).IsInEnum();
            RuleFor(x => x.StartDate).Custom((startDate, context) => {
                if (startDate >= context.InstanceToValidate.CompleteDate)
                {
                    context.AddFailure("Complete date must be greater than start date.");
                }
            });
            RuleFor(item => item.Priority).LessThan(6);
        }
    }
}
