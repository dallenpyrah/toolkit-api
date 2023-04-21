using System.Text.Json.Serialization;
using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.Contracts;

public class CreateUserRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
    
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }
    
    [JsonPropertyName("last_name")]
    public string LastName { get; set; }
    
    [JsonPropertyName("password")]
    public string Password { get; set; }
}