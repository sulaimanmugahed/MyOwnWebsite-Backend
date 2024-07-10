

namespace MyOwnWebsite.Application.Exceptions;
public class ApplicationNotFoundException : Exception
{
    public ApplicationNotFoundException(string errorMessage = "Not Found!",string? fieldName = null)
        : base(errorMessage)
    {    
        ErrorMessage = errorMessage;
        FieldName = fieldName;
    }
    public string ErrorMessage { get; init; }
    public string? FieldName { get; init; }

   
   
}
