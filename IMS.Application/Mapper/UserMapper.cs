using IMS.Application.DTOs.Users;
using IMS.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace IMS.Application.Mapper;
[Mapper]
public partial class UserMapper
{
    public partial AppUser ToEntity(AppUserRequest request);
    public partial UserResponse ToResponse(AppUser user);
}