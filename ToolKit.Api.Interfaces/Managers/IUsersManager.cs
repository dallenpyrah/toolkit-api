using ToolKit.Api.Contracts;
using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.Interfaces.Managers;

public interface IUsersManager
{
    ApiResponse<User> CreateUser(CreateUserRequest request);
    ApiResponse<User> GetUserById(int id);
}