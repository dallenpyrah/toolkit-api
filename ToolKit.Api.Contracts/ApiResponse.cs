using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.Contracts;

public class ApiResponse<T>
{
    public T? Body { get; set; }
    public string Message { get; set; }
}