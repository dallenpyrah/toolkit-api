using ToolKit.Api.Business.Exceptions;
using ToolKit.Api.Business.Extensions;
using ToolKit.Api.Contracts;
using ToolKit.Api.DataModel.Entities;
using ToolKit.Api.Interfaces.Managers;
using ToolKit.Api.Interfaces.Repositories;

namespace ToolKit.Api.Business.Managers;

public class UsersManager : IUsersManager
{
    private readonly IUsersRepository _usersRepository;

    public UsersManager(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }

    public async Task<User> CreateUser(CreateUserRequest request)
    {
        request.Validate();

        var isEmailAlreadyRegistered = await _usersRepository.IsEmailAlreadyRegistered(request.Email);
        if (isEmailAlreadyRegistered)
        {
            throw new EmailAlreadyRegisteredException("Email already registered.");
        }

        User user = request.ToUserEntity();
        return await _usersRepository.CreateUser(user);
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _usersRepository.GetUserById(id);

        if (user == null)
        {
            throw new UserNotFoundException("User not found.");
        }

        return user;
    }
}