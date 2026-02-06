using Domain.Enums;

namespace Domain.Entities;

public class Kennel : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public KennelSize Size { get; set; }
    public KennelZone Zone { get; set; }
    public KennelStatus Status { get; set; } = KennelStatus.Available;
    
    // For adjacency tracking
    public List<Guid> AdjacentKennelIds { get; set; } = new();
    
    // Current pet assignment
    public Guid? CurrentPetId { get; set; }
    public Pet? CurrentPet { get; set; }
    
    // Cleaning tracking
    public DateTime? CleaningStartedAtUtc { get; set; }
    public DateTime? CleaningUntilUtc { get; set; }
    
    // Navigation properties
    public ICollection<Stay> Stays { get; set; } = new List<Stay>();
}
