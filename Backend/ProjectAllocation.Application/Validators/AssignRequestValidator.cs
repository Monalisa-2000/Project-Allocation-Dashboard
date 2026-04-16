using FluentValidation;
using ProjectAllocation.Application.DTOs.Allocations;

namespace ProjectAllocation.Application.Validators;

public class AssignRequestValidator : AbstractValidator<AssignRequest>
{
    public AssignRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.ProjectIds)
            .NotEmpty();
    }
}
