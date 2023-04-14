using Microsoft.AspNetCore.Identity;

namespace Infrastructure.EntityFramework.Models;

public class UserData : IdentityUser
{
    public string? Country { get; set; }
    public string? City { get; set; }
    public string? Street { get; set; }
    public string? Zipcode { get; set; }

    public UserData(string userName) : base(userName)
    {
        
    }
}