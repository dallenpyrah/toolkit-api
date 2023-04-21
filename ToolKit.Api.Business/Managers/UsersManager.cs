using System.Net.Mail;
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
    public ApiResponse<User> CreateUser(CreateUserRequest request)
    {
        request.Validate();
        
        bool isEmailAlreadyRegistered = _usersRepository.IsEmailAlreadyRegistered(request.Email);
        if (isEmailAlreadyRegistered)
        {
            throw new EmailAlreadyRegisteredException("Email already registered.");
        }
        
        User user = request.ToUserEntity();
        user = _usersRepository.CreateUser(user);
        return user.ToApiResponse("User created successfully.");
    }

}