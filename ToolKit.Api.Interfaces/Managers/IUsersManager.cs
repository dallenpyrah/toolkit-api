using ToolKit.Api.Contracts;
using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.Interfaces.Managers;

public interface IUsersManager
{
    Task<User> CreateUser(CreateUserRequest request);
    Task<User> GetUserById(int id);
}