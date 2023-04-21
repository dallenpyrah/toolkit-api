using System.Net.Mail;
using System.Text.RegularExpressions;
using ToolKit.Api.Business.Exceptions;
using ToolKit.Api.Contracts;
using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.Business.Extensions;

public static class CreateUserRequestExtensions
{
    public static User ToUserEntity(this CreateUserRequest createUserRequest)
    {
        User user = new User
        {
            Email = createUserRequest.Email,
            FirstName = createUserRequest.FirstName,
            LastName = createUserRequest.LastName,
            Password = createUserRequest.Password,
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        return user;
    }
    
    public static void Validate(this CreateUserRequest request)
    {
        if (!IsValidEmail(request.Email))
        {
            throw new UserValidationException("Invalid email format.");
        }

        if (string.IsNullOrWhiteSpace(request.FirstName) || string.IsNullOrWhiteSpace(request.LastName))
        {
            throw new UserValidationException("First and last names must not be empty or contain only whitespace.");
        }

        if (!IsValidPassword(request.Password))
        {
            throw new UserValidationException("Invalid password. Password must meet complexity requirements.");
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            MailAddress mailAddress = new System.Net.Mail.MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }
        
        Regex hasUpperCase = new Regex(@"[A-Z]+");
        Regex hasLowerCase = new Regex(@"[a-z]+");
        Regex hasDigit = new Regex(@"\d+");
        Regex hasSpecialChar = new Regex(@"[\W_]+");
        
        const int minLength = 8;

        return
            hasUpperCase.IsMatch(password) &&
            hasLowerCase.IsMatch(password) &&
            hasDigit.IsMatch(password) &&
            hasSpecialChar.IsMatch(password) &&
            password.Length >= minLength;
    }

}