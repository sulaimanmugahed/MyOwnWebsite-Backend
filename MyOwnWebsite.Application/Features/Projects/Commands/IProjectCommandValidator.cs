using FluentValidation;

namespace MyOwnWebsite.Application.Features;

public class IProjectCommandValidator : AbstractValidator<IProjectCommand>
{
    public IProjectCommandValidator()
    {
        RuleFor(p => p.Title)
        .NotNull()
        .NotEmpty()
        .WithMessage("the title cant be null or empty");

        RuleFor(p => p.Description)
        .MaximumLength(4000)
        .WithMessage("the maximum chars of description is 4000");

    }

}
