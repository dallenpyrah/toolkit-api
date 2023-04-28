using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.Interfaces.Repositories;

public interface IUsersRepository
{
    Task<User> CreateUser(User user);
    Task<bool> IsEmailAlreadyRegistered(string userEmail);
    Task<User?> GetUserById(int id);
}