using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.Interfaces.Repositories;

public interface IUsersRepository
{
    User CreateUser(User user);
    bool IsEmailAlreadyRegistered(string userEmail);
}