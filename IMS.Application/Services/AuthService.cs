using IMS.Application.Abstractions;
using IMS.Application.DTOs.Users;
using IMS.Application.Mapper;
using IMS.Core.Entities;
using IMS.Core.Exceptions;
using Microsoft.AspNetCore.Identity;


namespace IMS.Application.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly UserMapper _mapper;
    private readonly IAppDbContext _context;
    public AuthService(UserManager<AppUser> userManager,UserMapper mapper,IAppDbContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }
    public async Task<UserResponse> AddNewUserAsync(AppUserRequest user)
    {
        var isExist = await _userManager.FindByEmailAsync(user.Email);
        if (isExist != null)
            throw new BadRequestException(ErrorMessages.UserAlreadyCreated);

        var appUser = _mapper.ToEntity(user);
        appUser.specialization = user.specialization;
        appUser.university = user.university;

        var result = await _userManager.CreateAsync(appUser, user.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"User creation failed: {errors}");
        }

        switch (user.role.ToLower())
        {
            case "manager":
                await _userManager.AddToRoleAsync(appUser, "Manager");
                break;

            case "trainer":
                await _userManager.AddToRoleAsync(appUser, "Trainer");

                var trainer = new Trainer
                {
                    User = appUser,
                    YearsOfExperience = user.YearsOfExperience
                };
                _context.Trainers.Add(trainer);
                break;

            case "trainee":
                await _userManager.AddToRoleAsync(appUser, "Trainee");

                var trainee = new Trainee
                {
                    User = appUser,
                    GraduationYear = user.GraduationYear ?? 0,
                    GitHubUsername = user.GitHubUsername,
                    LinkedInProfile = user.LinkedInProfile,
                    CVUrl = user.CVUrl
                };
                _context.Trainees.Add(trainee);
                break;

            case "teamlead":
                await _userManager.AddToRoleAsync(appUser, "TeamLead");

                var teamlead = new TeamLead
                {
                    User = appUser,
                };
                _context.TeamLeads.Add(teamlead);
                break;

            case "admin":
                var admin = new Admin
                {
                    User = appUser,
                };
                _context.Admins.Add(admin);
                await _userManager.AddToRoleAsync(appUser, "Admin");
                break;

            default:
                throw new BadRequestException(ErrorMessages.InvalidRole);
        }
        await _context.SaveChangesAsync();

        return _mapper.ToResponse(appUser);
    }
}
