namespace Domain.Entities;

public class Stay : BaseEntity
{
    public Guid PetId { get; set; }
    public Pet? Pet { get; set; }
    
    public Guid KennelId { get; set; }
    public Kennel? Kennel { get; set; }
    
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    
    // Auto-calculated: checkout + 2 hours cleaning downtime
    public DateTime? CleaningUntil { get; set; }
    
    public bool IsActive { get; set; }
}
