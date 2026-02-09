

namespace IMS.Core.Entities;

public class Trainee
{
    public AppUser User { get; set; }
    public int UserId { get; set; }
    public int GraduationYear { get; set; }
    public string  GitHubUsername { get; set; }
    public string LinkedInProfile { get; set; }
    public string CVUrl { get; set; }
}
