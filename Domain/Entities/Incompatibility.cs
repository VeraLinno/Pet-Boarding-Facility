using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class Incompatibility : BaseEntity
{
    public Guid PetAId { get; set; }
    public Pet? PetA { get; set; }
    
    public Guid PetBId { get; set; }
    public Pet? PetB { get; set; }
    
    public string RuleDescription { get; set; } = "NeverAdjacent";
}
