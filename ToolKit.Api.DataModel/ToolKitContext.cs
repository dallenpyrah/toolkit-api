using Microsoft.EntityFrameworkCore;
using ToolKit.Api.DataModel.Entities;

namespace ToolKit.Api.DataModel;

public class ToolKitContext : DbContext
{
    public ToolKitContext(DbContextOptions<ToolKitContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}