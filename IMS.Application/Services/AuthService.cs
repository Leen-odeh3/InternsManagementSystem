using IMS.Application.Abstractions;
using IMS.Core.Constants;
using IMS.Core.Entities;
using IMS.Core.Exceptions;
using IMS.Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace IMS.Application.Services;
public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITraineeRepository _traineeRepository;
    private readonly ITrainerRepository _trainerRepository;

    public AuthService(
        UserManager<AppUser> userManager,
        IUnitOfWork unitOfWork,
        ITraineeRepository traineeRepository, ITrainerRepository trainerRepository)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _traineeRepository = traineeRepository;
        _trainerRepository = trainerRepository;
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
                        await _trainerRepository.AddAsync(trainer);
                    }
                    break;

                case StaticRole.Trainee:
                    await _userManager.AddToRoleAsync(appUser, StaticRole.Trainee);

                    if (trainee != null)
                    {
                        trainee.UserId = appUser.Id;
                        await _traineeRepository.AddAsync(trainee);
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

            return appUser;
        });
    }

}
