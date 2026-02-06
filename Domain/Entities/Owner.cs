using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Owner : BaseEntity
{
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Premium level is required.")]
    public PremiumLevel PremiumLevel { get; set; } = PremiumLevel.Standard;
    
    // Navigation properties
    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
}
