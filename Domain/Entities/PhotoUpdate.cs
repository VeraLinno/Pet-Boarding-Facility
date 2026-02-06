using Domain.Enums;

namespace Domain.Entities;

public class PhotoUpdate : BaseEntity
{
    public Guid PetId { get; set; }
    public Pet? Pet { get; set; }
    
    public DateTime Date { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public bool SentToOwner { get; set; }
    public UpdateType UpdateType { get; set; }
}
