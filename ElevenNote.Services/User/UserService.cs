using ElevenNote.Data;
using ElevenNote.Data.Entities;
using ElevenNote.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ElevenNote.Services.User;

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<UserEntity> _userManager;
    private readonly SignInManager<UserEntity> _signInManager;

    public UserService(
        ApplicationDbContext context,
        UserManager<UserEntity> userManager,
        SignInManager<UserEntity> signInManager)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> LoginAsync(UserLogin model)
    {
        // verifies the user exists by the username
        var UserEntity = await _userManager.FindByNameAsync(model.UserName);
        if (UserEntity is null)
            return false;

        // verifies the correct password was given
        var passwordHasher = new PasswordHasher<UserEntity>();
        var verifyPasswordResult = passwordHasher.VerifyHashedPassword(UserEntity, UserEntity.Password, model.Password);
        if (verifyPasswordResult == PasswordVerificationResult.Failed)
            return false;

        // finally, since the user exists and the password is correct, sign in the user
        await _signInManager.SignInAsync(UserEntity, true);
        return true;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<bool> RegisterUserAsync(UserRegister model)
    {
        if (await GetUserByEmailAsync(model.Email) != null || await GetUserByUsernameAsync(model.Username) != null)
            return false;

        UserEntity entity = new()
        {
            Email = model.Email,
            Username = model.Username,
            DateCreated = DateTime.Now
        };

        var passwordHasher = new PasswordHasher<UserEntity>();
        entity.Password = passwordHasher.HashPassword(entity, model.Password);

        var createResult = await _userManager.CreateAsync(entity);
        return createResult.Succeeded; //replaces the following

        // _context.Users.Add(entity);
        // int numberOfChanges = await _context.SaveChangesAsync();

        // return numberOfChanges == 1;
    }

    public async Task<UserDetail?> GetUserByIdAsync(int userId)
    {
        var entity = await _context.Users.FindAsync(userId);
        if (entity is null)
            return null;

        UserDetail model = new()
        {
            Id = entity.Id,
            Email = entity.Email,
            Username = entity.Username,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            DateCreated = entity.DateCreated
        };

        return model;
    }

    private async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == email.ToLower());
    }
    private async Task<UserEntity?> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(user => user.Username.ToLower() == username.ToLower());
    }
}