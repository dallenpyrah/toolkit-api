using System.ComponentModel.DataAnnotations;

namespace ToolKit.Api.DataModel.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)] 
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Password { get; set; } = null!;

    [Required]
    public bool IsActive { get; set; }
    
    [Required]
    public DateTime CreatedDate { get; set; }
    
    [Required]
    public DateTime ModifiedDate { get; set; }
}