using System.Diagnostics;
using IMS.Application.Abstractions;
using IMS.Application.DI;
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

    public AuthService(
        UserManager<AppUser> userManager,
        UserMapper mapper,
        IAppDbContext context)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
    }

    public async Task<UserResponse> AddNewUserAsync(AppUserRequest user)
    {
        using var activity =
            Observability.ActivitySource.StartActivity("AuthService.AddNewUser");

        activity?.SetTag("user.email", user.Email);
        activity?.SetTag("user.role", user.role);

        // 1️⃣ Check if user exists
        var isExist = await _userManager.FindByEmailAsync(user.Email);
        if (isExist != null)
        {
            activity?.SetStatus(ActivityStatusCode.Error, "User already exists");
            throw new BadRequestException(ErrorMessages.UserAlreadyCreated);
        }

        // 2️⃣ Create identity user
        var appUser = _mapper.ToEntity(user);
        appUser.specialization = user.specialization;
        appUser.university = user.university;

        var result = await _userManager.CreateAsync(appUser, user.Password);
        if (!result.Succeeded)
        {
            activity?.SetStatus(ActivityStatusCode.Error, "Identity creation failed");
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new BadRequestException($"User creation failed: {errors}");
        }

        activity?.SetTag("identity.created", true);

        // 3️⃣ Assign role + domain entity
        using (var roleActivity =
               Observability.ActivitySource.StartActivity("Assign User Role"))
        {
            roleActivity?.SetTag("role", user.role);

            switch (user.role.ToLower())
            {
                case "manager":
                    await _userManager.AddToRoleAsync(appUser, "Manager");
                    break;

                case "trainer":
                    await _userManager.AddToRoleAsync(appUser, "Trainer");
                    _context.Trainers.Add(new Trainer
                    {
                        User = appUser,
                        YearsOfExperience = user.YearsOfExperience
                    });
                    break;

                case "trainee":
                    await _userManager.AddToRoleAsync(appUser, "Trainee");
                    _context.Trainees.Add(new Trainee
                    {
                        User = appUser,
                        GraduationYear = user.GraduationYear ?? 0,
                        GitHubUsername = user.GitHubUsername,
                        LinkedInProfile = user.LinkedInProfile,
                        CVUrl = user.CVUrl
                    });
                    break;

                case "teamlead":
                    await _userManager.AddToRoleAsync(appUser, "TeamLead");
                    _context.TeamLeads.Add(new TeamLead
                    {
                        User = appUser
                    });
                    break;

                case "admin":
                    await _userManager.AddToRoleAsync(appUser, "Admin");
                    _context.Admins.Add(new Admin
                    {
                        User = appUser
                    });
                    break;

                default:
                    roleActivity?.SetStatus(ActivityStatusCode.Error, "Invalid role");
                    throw new BadRequestException(ErrorMessages.InvalidRole);
            }
        }

        // 4️⃣ Save changes
        await _context.SaveChangesAsync();
        activity?.SetTag("db.saved", true);

        // 5️⃣ Response
        var res = _mapper.ToResponse(appUser);
        res.UserId = appUser.Id;
        res.Role = user.role;

        return res;
    }
}
