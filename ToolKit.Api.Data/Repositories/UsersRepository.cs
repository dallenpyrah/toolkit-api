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


    public User CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }

    public bool IsEmailAlreadyRegistered(string userEmail)
    {
        return _context.Users.Any(u => u.Email == userEmail);
    }

    public User? GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(u => u.Id == id);
    }
}