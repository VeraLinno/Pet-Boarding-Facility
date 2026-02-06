using Application.Abstractions;
using Domain.Entities;
using Domain.Enums;
using Domain.Rules;

namespace Application.Services;

public sealed class KennelAllocationService
{
    private readonly IPetRepository _pets;
    private readonly IKennelRepository _kennels;
    private readonly IStayRepository _stays;
    private readonly IIncompatibilityRepository _incompatibilities;

    public KennelAllocationService(
        IPetRepository pets, 
        IKennelRepository kennels,
        IStayRepository stays,
        IIncompatibilityRepository incompatibilities)
    {
        _pets = pets;
        _kennels = kennels;
        _stays = stays;
        _incompatibilities = incompatibilities;
    }

    public async Task AssignPetToKennelAsync(Guid petId, Guid kennelId, CancellationToken ct = default)
    {
        var pet = await _pets.GetAsync(petId, ct) ?? throw new InvalidOperationException("Pet not found.");
        var kennel = await _kennels.GetAsync(kennelId, ct) ?? throw new InvalidOperationException("Kennel not found.");

        var allKennels = await _kennels.ListAsync(ct);
        var byId = allKennels.ToDictionary(k => k.Id, k => k);

        var incompatibilities = await _incompatibilities.ListAsync(ct);
        var adjacencyRule = new AdjacencyRule(incompatibilities);

        // Domain rules
        KennelAssignmentRules.EnsureCanAssign(pet, kennel, DateTimeOffset.UtcNow);

        // Aggressive dog cannot be adjacent to other pets (stronger than blacklist)
        if (pet.Species == PetSpecies.Dog && pet.Aggressive)
        {
            foreach (var adjacentId in kennel.AdjacentKennelIds)
            {
                if (!byId.TryGetValue(adjacentId, out var adjacent))
                    continue;

                if (adjacent.Status == KennelStatus.Occupied && adjacent.CurrentPetId is not null)
                    throw new InvalidOperationException("Rule violated: Aggressive dog cannot be adjacent to other pets.");
            }
        }

        // Blacklist adjacency
        adjacencyRule.EnsureNoBlacklistedAdjacency(pet.Id, kennel, byId);

        // Apply state
        kennel.Status = KennelStatus.Occupied;
        kennel.CurrentPetId = pet.Id;
        kennel.CleaningStartedAtUtc = null;
        kennel.CleaningUntilUtc = null;

        await _kennels.UpdateAsync(kennel, ct);
    }

    public async Task CheckOutPetAsync(Guid kennelId, CancellationToken ct = default)
    {
        var kennel = await _kennels.GetAsync(kennelId, ct) ?? throw new InvalidOperationException("Kennel not found.");

        if (kennel.Status != KennelStatus.Occupied)
            throw new InvalidOperationException("Kennel is not occupied.");

        // Set cleaning status with 2-hour downtime
        kennel.Status = KennelStatus.Cleaning;
        kennel.CurrentPetId = null;
        kennel.CleaningStartedAtUtc = DateTime.UtcNow;
        kennel.CleaningUntilUtc = DateTime.UtcNow.AddHours(2);

        await _kennels.UpdateAsync(kennel, ct);
    }

    public async Task UpdateKennelCleaningStatusAsync(CancellationToken ct = default)
    {
        var kennels = await _kennels.ListAsync(ct);
        var now = DateTime.UtcNow;

        foreach (var kennel in kennels)
        {
            if (kennel.Status == KennelStatus.Cleaning && 
                kennel.CleaningUntilUtc.HasValue && 
                kennel.CleaningUntilUtc.Value <= now)
            {
                kennel.Status = KennelStatus.Available;
                kennel.CleaningStartedAtUtc = null;
                kennel.CleaningUntilUtc = null;
                await _kennels.UpdateAsync(kennel, ct);
            }
        }
    }

    public async Task<List<Kennel>> GetAvailableKennelsAsync(PetSpecies species, PetSize size, bool isAggressive, CancellationToken ct = default)
    {
        var kennels = await _kennels.ListAsync(ct);
        var now = DateTime.UtcNow;

        return kennels.Where(k =>
        {
            // Check if kennel is available
            if (k.Status != KennelStatus.Available)
                return false;

            // Check cleaning downtime
            if (k.CleaningUntilUtc.HasValue && k.CleaningUntilUtc.Value > now)
                return false;

            // Zone rules
            if (species == PetSpecies.Cat && k.Zone != KennelZone.Cat)
                return false;

            if (species == PetSpecies.Dog && k.Zone == KennelZone.Cat)
                return false;

            if (isAggressive && k.Zone != KennelZone.Isolation)
                return false;

            // Size compatibility
            if (size == PetSize.Large && k.Size != KennelSize.Large)
                return false;

            return true;
        }).ToList();
    }
}
