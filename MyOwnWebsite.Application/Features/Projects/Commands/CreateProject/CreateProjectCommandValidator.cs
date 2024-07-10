using FluentValidation;

namespace MyOwnWebsite.Application.Features;

public class CreateProjectCommandValidator:AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        Include(new IProjectCommandValidator());
    }
}
