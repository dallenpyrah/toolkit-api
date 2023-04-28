using Microsoft.EntityFrameworkCore;
using ToolKit.Api.DataModel;
using ToolKit.Api.DataModel.Entities;
using ToolKit.Api.Interfaces.Repositories;

namespace ToolKit.Api.Data.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly ToolKitContext _context;

    public UsersRepository(ToolKitContext context)
    {
        _context = context;
    }


    public async Task<User> CreateUser(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> IsEmailAlreadyRegistered(string userEmail)
    {
        return await _context.Users.AnyAsync(u => u.Email == userEmail);
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }
}