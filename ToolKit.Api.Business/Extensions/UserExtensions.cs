using ToolKit.Api.Contracts;
using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.Business.Extensions;

public static class UserExtensions
{
    public static ApiResponse<User> ToApiResponse(this User user, string message)
    {
        ApiResponse<User> apiResponse = new ApiResponse<User>
        {
            Body = user,
            Message = message
        };

        return apiResponse;
    }
}