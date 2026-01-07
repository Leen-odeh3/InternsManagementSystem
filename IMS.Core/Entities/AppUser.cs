using Microsoft.AspNetCore.Identity;

namespace IMS.Core.Entities;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; } = false;
    public string ProfilePictureUrl { get; set; }
    public string specialization { get; set; }
    public string university { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;  
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;    
    public Hr? Hr { get; set; }
    public Trainer? Trainer { get; set; }
    public Trainee? Trainee { get; set; }    
    public Manager? Manager { get; set; }
    public TeamLead? TeamLead { get; set; }
}
