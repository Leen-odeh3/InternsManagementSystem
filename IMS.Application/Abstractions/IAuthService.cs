using IMS.Application.DTOs.Users;
using IMS.Core.Entities;

namespace IMS.Application.Abstractions;
public interface IAuthService
{
    Task<UserResponse> AddNewUserAsync(AppUserRequest user);
}
