using News.Api.Models.Dtos;

namespace News.Api.Validators;

public class HelloRequestValidator
{
    public HelloRequestValidator()
    {
    }
}

public static class ValidationHelper
{
    public static (bool IsValid, List<string> Errors) ValidateName(string name)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(name))
        {
            errors.Add("Name is required");
        }
        else
        {
            if (name.Length < 2)
                errors.Add("Name must be at least 2 characters long");
            
            if (name.Length > 50)
                errors.Add("Name cannot exceed 50 characters");
            
            if (!System.Text.RegularExpressions.Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
                errors.Add("Name can only contain letters and spaces");
        }

        return (errors.Count == 0, errors);
    }
}
