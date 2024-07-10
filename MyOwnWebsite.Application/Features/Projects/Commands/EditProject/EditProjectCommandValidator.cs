using FluentValidation;

namespace MyOwnWebsite.Application.Features;

public class EditProjectCommandValidator:AbstractValidator<EditProjectCommand>
{
    public EditProjectCommandValidator()
    {
        Include(new IProjectCommandValidator());
    }

}
