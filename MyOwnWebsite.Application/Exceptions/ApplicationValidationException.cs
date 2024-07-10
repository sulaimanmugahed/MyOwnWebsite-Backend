using FluentValidation.Results;


namespace MyOwnWebsite.Application.Exceptions;


public class ApplicationValidationException : Exception
{
    public ApplicationValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures.ToList();
    }
    public List<ValidationFailure> Errors { get; }

}

