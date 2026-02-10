using IMS.Application.Abstractions;
using IMS.Core.Constants;
using IMS.Core.Entities;
using IMS.Core.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace IMS.Application.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IAppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AuthService(
        UserManager<AppUser> userManager,
        IAppDbContext context,
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task<AppUser> AddNewUserAsync(
    AppUser appUser,
    string password,
    string role,
    Trainer? trainer,
    Trainee? trainee)
    {

        return await _unitOfWork.ExecuteInTransactionAsync(async () =>
        {
            var isExist = await _userManager.FindByEmailAsync(appUser.Email);

            if (isExist != null)
            {
                throw new BadRequestException(ErrorMessages.UserAlreadyCreated);
            }

            var result = await _userManager.CreateAsync(appUser, password);

            if (!result.Succeeded)
            {
                throw new BadRequestException(
                    string.Join(",", result.Errors.Select(e => e.Description)));
            }

            switch (role?.Trim())
            {
                case StaticRole.Manager:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.Manager);
                    break;

                case StaticRole.Trainer:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.Trainer);

                    if (trainer != null)
                    {
                        trainer.UserId = appUser.Id;
                        _context.Trainers.Add(trainer);
                    }
                    break;

                case StaticRole.Trainee:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.Trainee);

                    if (trainee != null)
                    {
                        trainee.UserId = appUser.Id;
                        _context.Trainees.Add(trainee);
                    }
                    break;

                case StaticRole.TeamLead:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.TeamLead);
                    break;

                case StaticRole.Admin:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.Admin);
                    break;

                default:
                    throw new BadRequestException(ErrorMessages.InvalidRole);
            }

            await _context.SaveChangesAsync();
            return appUser;
        });
    }

}
