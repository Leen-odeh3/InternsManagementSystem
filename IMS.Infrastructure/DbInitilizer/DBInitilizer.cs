using IMS.Core.Constants;
using IMS.Core.Entities;
using IMS.Core.Exceptions;
using IMS.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IMS.Infrastructure.DbInitilizer;

public class DBInitilizer : IDBInitilizer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly AppDbContext _context;
    private readonly ILogger<DBInitilizer> _logger;
    public DBInitilizer(
     AppDbContext context,
     RoleManager<AppRole> roleManager,
     UserManager<AppUser> userManager,
     ILogger<DBInitilizer> logger)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
    }
    public async Task Initialize()
    {
        try
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
                await _context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Migration failed while initializing database");
        }


        string[] roles =
        {
        StaticRole.Manager,
        StaticRole.Trainer,
        StaticRole.Trainee,
        StaticRole.TeamLead,
        StaticRole.Admin,
        StaticRole.SuperAdmin
    };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new AppRole { Name = role });
        }

        var existingUser = await _userManager.FindByNameAsync("super.admin");

        if (existingUser is not null)
            return;

        var user = new AppUser
        {
            UserName = "super.admin",
            NormalizedUserName = "SUPER.ADMIN",
            Email = "superAdmin@company.com",
            NormalizedEmail = "SUPERADMIN@COMPANY.COM",
            FirstName = "Super",
            LastName = "Admin",
            ProfilePictureUrl = "default.png",
            Specialization = "System",
            University = "Internal",
            IsActive = true,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, "SuperAdmin@123");

        if (!result.Succeeded)
        {
            var errors = string.Join(" | ",
                result.Errors.Select(e => e.Description));

            throw new Core.Exceptions.InvalidOperationException(
                $"{ErrorMessages.FailedToCreateUser}: {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, StaticRole.SuperAdmin);

        if (!roleResult.Succeeded)
            throw new Core.Exceptions.InvalidOperationException(
                ErrorMessages.FailedToAssignRole);
    }

}