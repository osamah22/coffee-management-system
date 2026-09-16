using Microsoft.AspNetCore.Identity;

namespace Application.Models;

public sealed class User : IdentityUser
{
    public bool EmailNotification { get; set; }
}