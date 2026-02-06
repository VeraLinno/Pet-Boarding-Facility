namespace Domain.Entities;

public class FeedingSchedule : BaseEntity
{
    public Guid PetId { get; set; }
    public Pet? Pet { get; set; }
    
    public string FoodType { get; set; } = string.Empty;
    public string Restrictions { get; set; } = string.Empty;
    public int TimesPerDay { get; set; }
    public string Notes { get; set; } = string.Empty;
}
