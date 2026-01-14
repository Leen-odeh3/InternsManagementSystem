using IMS.Application.Abstractions;
using IMS.Application.DI;
using IMS.Application.DTOs.Users;
using IMS.Application.Mapper;
using IMS.Core.Constants;
using IMS.Core.Entities;
using IMS.Core.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace IMS.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly UserMapper _mapper;
    private readonly IAppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        UserManager<AppUser> userManager,
        UserMapper mapper,
        IAppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _mapper = mapper;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserResponse> AddNewUserAsync(AppUserRequest user)
    {
        using var activity =
            Observability.ActivitySource.StartActivity("AuthService.AddNewUser");
        await _unitOfWork.BeginTransactionAsync();

        activity?.SetTag("user.email", user.Email);
        activity?.SetTag("user.role", user.Role);

        var isExist = await _userManager.FindByEmailAsync(user.Email);
        if (isExist != null)
        {
            activity?.SetStatus(ActivityStatusCode.Error, "User already exists");
            throw new BadRequestException(ErrorMessages.UserAlreadyCreated);
        }

        try
        {
            var appUser = _mapper.ToEntity(user);
            appUser.Specialization = user.Specialization;
            appUser.University = user.University;

            var result = await _userManager.CreateAsync(appUser, user.Password);
            if (!result.Succeeded)
            {
                activity?.SetStatus(ActivityStatusCode.Error, "Identity creation failed");
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new BadRequestException($"User creation failed: {errors}");
            }

            activity?.SetTag("identity.created", true);

            using (var roleActivity =
                   Observability.ActivitySource.StartActivity("Assign User Role"))
            {
                roleActivity?.SetTag("role", user.Role);

                switch (user.Role?.Trim())
                {
                    case StaticRole.Manager:
                        await _userManager.AddToRoleAsync(appUser, "Manager");
                        break;

                    case StaticRole.Trainer:
                        await _userManager.AddToRoleAsync(appUser, "Trainer");
                        _context.Trainers.Add(new Trainer
                        {
                            User = appUser,
                            YearsOfExperience = user.YearsOfExperience
                        });
                        break;

                    case StaticRole.Trainee:
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

                    case StaticRole.TeamLead:
                        await _userManager.AddToRoleAsync(appUser, "TeamLead");
                        _context.TeamLeads.Add(new TeamLead
                        {
                            User = appUser
                        });
                        break;

                    case StaticRole.Admin:
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

            await _context.SaveChangesAsync();
            activity?.SetTag("db.saved", true);

            var res = _mapper.ToResponse(appUser);
            res.UserId = appUser.Id;
            res.Role = user.Role;

        return res;
        }
                catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            await _unitOfWork.RollbackAsync();
            throw;
        }
        
    }
}
