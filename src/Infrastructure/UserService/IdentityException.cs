using Microsoft.AspNetCore.Identity;

namespace Infrastructure.UserService;

public class IdentityException : Exception
{
    public List<string> Errors { get; }
    
    public IdentityException(IEnumerable<IdentityError> errors) : base(Format(errors))
    {
        Errors = errors.Select(e => e.Description).ToList();
    }

    private static string Format(IEnumerable<IdentityError> errors)
    {
        List<string> stringErrors = errors.Select(e => e.Description).ToList();
        return $"Action failed: {string.Join(", ", stringErrors)}";
    }
}