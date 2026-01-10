

namespace IMS.Application.DTOs.Users;
public class AppUserRequest
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsActive { get; set; } = false;
    public string ProfilePictureUrl { get; set; }
    public string specialization { get; set; }
    public string university { get; set; }
    public string role { get; set; }
    public string Password { get; set; }
    public int? GraduationYear { get; set; }
    public string? GitHubUsername { get; set; }
    public string? LinkedInProfile { get; set; }
    public string? CVUrl { get; set; }
    public string? YearsOfExperience { get; set; }

}
