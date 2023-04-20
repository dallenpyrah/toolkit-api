using Microsoft.EntityFrameworkCore;

namespace ToolKit.Api.DataModel;

public class ToolKitContext : DbContext
{
    public ToolKitContext(DbContextOptions<ToolKitContext> options) : base(options)
    {
    }
}