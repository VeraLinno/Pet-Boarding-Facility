using Domain.Entities;
using Domain.Enums;

namespace Domain.Rules;

public static class KennelAssignmentRules
{
    public static void EnsureCanAssign(Pet pet, Kennel kennel, DateTimeOffset now, IEnumerable<Incompatibility>? incompatibilities = null)
    {
        // Zone rules
        if (pet.Species == PetSpecies.Cat && kennel.Zone != KennelZone.Cat)
            throw new InvalidOperationException("Cats must only be placed in cat kennels.");
        
        if (pet.Species == PetSpecies.Dog && kennel.Zone == KennelZone.Cat)
            throw new InvalidOperationException("Dogs cannot be placed in cat kennels.");
        
        // Kennel must be available
        if (kennel.Status != KennelStatus.Available)
            throw new InvalidOperationException($"Kennel is not available. Current status: {kennel.Status}");
        
        // Check cleaning downtime
        if (kennel.CleaningUntilUtc.HasValue && kennel.CleaningUntilUtc.Value > now)
            throw new InvalidOperationException("Kennel is still being cleaned.");
        
        // Size compatibility (optional rule - can be relaxed)
        if (pet.Size == PetSize.Large && kennel.Size != KennelSize.Large)
            throw new InvalidOperationException("Large pets require large kennels.");
        
        // Incompatible pets adjacency rule
        if (incompatibilities != null && kennel.CurrentPetId.HasValue)
        {
            var currentPetId = kennel.CurrentPetId.Value;
            var isIncompatible = incompatibilities.Any(i =>
                (i.PetAId == pet.Id && i.PetBId == currentPetId) ||
                (i.PetBId == pet.Id && i.PetAId == currentPetId));
            
            if (isIncompatible)
                throw new InvalidOperationException("Cannot assign pet to kennel adjacent to incompatible pet.");
        }
    }
    
    public static void EnsureNoIncompatibleAdjacency(Pet pet, Kennel kennel, IDictionary<Guid, Kennel> allKennels, IEnumerable<Incompatibility> incompatibilities)
    {
        foreach (var adjacentId in kennel.AdjacentKennelIds)
        {
            if (!allKennels.TryGetValue(adjacentId, out var adjacent))
                continue;
            
            if (!adjacent.CurrentPetId.HasValue)
                continue;
            
            var adjacentPetId = adjacent.CurrentPetId.Value;
            var isIncompatible = incompatibilities.Any(i =>
                (i.PetAId == pet.Id && i.PetBId == adjacentPetId) ||
                (i.PetBId == pet.Id && i.PetAId == adjacentPetId));
            
            if (isIncompatible)
                throw new InvalidOperationException($"Cannot assign pet adjacent to incompatible pet in kennel {adjacent.Name}.");
        }
    }
}
