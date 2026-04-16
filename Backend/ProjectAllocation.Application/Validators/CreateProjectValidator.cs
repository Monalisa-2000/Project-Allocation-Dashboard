using FluentValidation;
using ProjectAllocation.Application.DTOs.Projects;

namespace ProjectAllocation.Application.Validators;

public class CreateProjectValidator : AbstractValidator<CreateProjectRequest>
{
    public CreateProjectValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
