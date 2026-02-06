namespace Domain.Entities;

public class Medication : BaseEntity
{
    public Guid PetId { get; set; }
    public Pet? Pet { get; set; }
    
    public string Name { get; set; } = string.Empty;
    public TimeSpan Time { get; set; } // e.g., 8:00 AM, 6:00 PM
    public string SpecialCondition { get; set; } = string.Empty; // e.g., "Before thunderstorms"
    public bool Required { get; set; }
    
    // Navigation properties
    public ICollection<MedicationLog> Logs { get; set; } = new List<MedicationLog>();
}
