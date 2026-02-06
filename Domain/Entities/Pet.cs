using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Pet : BaseEntity
{
    [Required(ErrorMessage = "Pet name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
    public string Name { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Species is required.")]
    public Enums.PetSpecies Species { get; set; }
    

    [Required(ErrorMessage = "Size is required.")]
    public Enums.PetSize Size { get; set; }
    
    public bool Aggressive { get; set; }
    
    public string DietaryRestrictions { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Owner is required.")]
    public Guid OwnerId { get; set; }
    public Owner? Owner { get; set; }
    
    public string Notes { get; set; } = string.Empty;
    
    public Guid? KennelId { get; set; }
    public Kennel? Kennel { get; set; }
    
    // Navigation properties
    public ICollection<Stay> Stays { get; set; } = new List<Stay>();
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();
    public ICollection<FeedingSchedule> FeedingSchedules { get; set; } = new List<FeedingSchedule>();
    public ICollection<PhotoUpdate> PhotoUpdates { get; set; } = new List<PhotoUpdate>();
}
