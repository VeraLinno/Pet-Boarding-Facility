namespace Domain.Entities;

public class MedicationLog : BaseEntity
{
    public Guid MedicationId { get; set; }
    public Medication? Medication { get; set; }
    
    public DateOnly Date { get; set; }
    public TimeSpan ScheduledTime { get; set; }
    public bool Given { get; set; }
    public DateTime? GivenAt { get; set; }
    public string StaffInitials { get; set; } = string.Empty;
}
