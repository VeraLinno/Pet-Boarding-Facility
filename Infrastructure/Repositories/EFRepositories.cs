using Application.Abstractions;
using DAL;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class EFOwnerRepository : IOwnerRepository
{
    private readonly AppDbContext _context;
    public EFOwnerRepository(AppDbContext context) => _context = context;

    public async Task<List<Owner>> ListAsync(CancellationToken ct = default) 
        => await _context.Owners.ToListAsync(ct);
    public async Task<Owner?> GetAsync(Guid id, CancellationToken ct = default) 
        => await _context.Owners.FirstOrDefaultAsync(o => o.Id == id, ct);
    public async Task<List<Owner>> SearchAsync(string searchTerm, CancellationToken ct = default)
    {
        var term = searchTerm.ToLower();
        return await _context.Owners
            .Where(o => o.Name.ToLower().Contains(term) || o.Email.ToLower().Contains(term))
            .ToListAsync(ct);
    }
    public async Task AddAsync(Owner owner, CancellationToken ct = default)
    {
        await _context.Owners.AddAsync(owner, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(Owner owner, CancellationToken ct = default)
    {
        _context.Owners.Update(owner);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == id, ct);
        if (owner != null)
        {
            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class EFPetRepository : IPetRepository
{
    private readonly AppDbContext _context;
    public EFPetRepository(AppDbContext context) => _context = context;

    public async Task<List<Pet>> ListAsync(CancellationToken ct = default) 
        => await _context.Pets.Include(p => p.Owner).ToListAsync(ct);
    public async Task<Pet?> GetAsync(Guid id, CancellationToken ct = default) 
        => await _context.Pets.Include(p => p.Owner).FirstOrDefaultAsync(p => p.Id == id, ct);
    public async Task<List<Pet>> SearchAsync(string searchTerm, CancellationToken ct = default)
    {
        var term = searchTerm.ToLower();
        return await _context.Pets
            .Include(p => p.Owner)
            .Where(p => p.Name.ToLower().Contains(term) || 
                        p.Breed.ToLower().Contains(term) || 
                        p.Owner.Name.ToLower().Contains(term))
            .ToListAsync(ct);
    }
    public async Task AddAsync(Pet pet, CancellationToken ct = default)
    {
        await _context.Pets.AddAsync(pet, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(Pet pet, CancellationToken ct = default)
    {
        _context.Pets.Update(pet);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var pet = await _context.Pets.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (pet != null)
        {
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class EFKennelRepository : IKennelRepository
{
    private readonly AppDbContext _context;
    public EFKennelRepository(AppDbContext context) => _context = context;

    public async Task<List<Kennel>> ListAsync(CancellationToken ct = default) 
        => await _context.Kennels.Include(k => k.CurrentPet).ToListAsync(ct);
    public async Task<Kennel?> GetAsync(Guid id, CancellationToken ct = default) 
        => await _context.Kennels.Include(k => k.CurrentPet).FirstOrDefaultAsync(k => k.Id == id, ct);
    public async Task<List<Kennel>> SearchAsync(string searchTerm, CancellationToken ct = default)
    {
        var term = searchTerm.ToLower();
        return await _context.Kennels
            .Include(k => k.CurrentPet)
            .Where(k => k.Name.ToLower().Contains(term))
            .ToListAsync(ct);
    }
    public async Task AddAsync(Kennel kennel, CancellationToken ct = default)
    {
        await _context.Kennels.AddAsync(kennel, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(Kennel kennel, CancellationToken ct = default)
    {
        _context.Kennels.Update(kennel);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var kennel = await _context.Kennels.FirstOrDefaultAsync(k => k.Id == id, ct);
        if (kennel != null)
        {
            _context.Kennels.Remove(kennel);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class EFStayRepository : IStayRepository
{
    private readonly AppDbContext _context;
    public EFStayRepository(AppDbContext context) => _context = context;

    public async Task<List<Stay>> ListAsync(CancellationToken ct = default) 
        => await _context.Stays.Include(s => s.Pet).Include(s => s.Kennel).ToListAsync(ct);
    public async Task<Stay?> GetAsync(Guid id, CancellationToken ct = default) 
        => await _context.Stays.Include(s => s.Pet).Include(s => s.Kennel).FirstOrDefaultAsync(s => s.Id == id, ct);
    public async Task<List<Stay>> GetActiveStaysAsync(CancellationToken ct = default) 
        => await _context.Stays.Include(s => s.Pet).Include(s => s.Kennel).Where(s => s.IsActive).ToListAsync(ct);
    public async Task AddAsync(Stay stay, CancellationToken ct = default)
    {
        await _context.Stays.AddAsync(stay, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(Stay stay, CancellationToken ct = default)
    {
        _context.Stays.Update(stay);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var stay = await _context.Stays.FirstOrDefaultAsync(s => s.Id == id, ct);
        if (stay != null)
        {
            _context.Stays.Remove(stay);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class EFMedicationRepository : IMedicationRepository
{
    private readonly AppDbContext _context;
    public EFMedicationRepository(AppDbContext context) => _context = context;

    public async Task<List<Medication>> ListAsync(CancellationToken ct = default) 
        => await _context.Medications.Include(m => m.Pet).ToListAsync(ct);
    public async Task<Medication?> GetAsync(Guid id, CancellationToken ct = default) 
        => await _context.Medications.Include(m => m.Pet).FirstOrDefaultAsync(m => m.Id == id, ct);
    public async Task<List<Medication>> GetByPetIdAsync(Guid petId, CancellationToken ct = default) 
        => await _context.Medications.Where(m => m.PetId == petId).ToListAsync(ct);
    public async Task AddAsync(Medication medication, CancellationToken ct = default)
    {
        await _context.Medications.AddAsync(medication, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(Medication medication, CancellationToken ct = default)
    {
        _context.Medications.Update(medication);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var medication = await _context.Medications.FirstOrDefaultAsync(m => m.Id == id, ct);
        if (medication != null)
        {
            _context.Medications.Remove(medication);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class EFMedicationLogRepository : IMedicationLogRepository
{
    private readonly AppDbContext _context;
    public EFMedicationLogRepository(AppDbContext context) => _context = context;

    public async Task<List<MedicationLog>> ListAsync(CancellationToken ct = default) 
        => await _context.MedicationLogs.Include(m => m.Medication).ThenInclude(m => m!.Pet).ToListAsync(ct);
    public async Task<MedicationLog?> GetAsync(Guid id, CancellationToken ct = default) 
        => await _context.MedicationLogs.Include(m => m.Medication).FirstOrDefaultAsync(m => m.Id == id, ct);
    public async Task<List<MedicationLog>> GetByMedicationIdAsync(Guid medicationId, CancellationToken ct = default) 
        => await _context.MedicationLogs.Where(m => m.MedicationId == medicationId).ToListAsync(ct);
    public async Task AddAsync(MedicationLog log, CancellationToken ct = default)
    {
        await _context.MedicationLogs.AddAsync(log, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(MedicationLog log, CancellationToken ct = default)
    {
        _context.MedicationLogs.Update(log);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var log = await _context.MedicationLogs.FirstOrDefaultAsync(m => m.Id == id, ct);
        if (log != null)
        {
            _context.MedicationLogs.Remove(log);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class EFFeedingScheduleRepository : IFeedingScheduleRepository
{
    private readonly AppDbContext _context;
    public EFFeedingScheduleRepository(AppDbContext context) => _context = context;

    public async Task<List<FeedingSchedule>> ListAsync(CancellationToken ct = default) 
        => await _context.FeedingSchedules.Include(f => f.Pet).ToListAsync(ct);
    public async Task<FeedingSchedule?> GetAsync(Guid id, CancellationToken ct = default) 
        => await _context.FeedingSchedules.Include(f => f.Pet).FirstOrDefaultAsync(f => f.Id == id, ct);
    public async Task<List<FeedingSchedule>> GetByPetIdAsync(Guid petId, CancellationToken ct = default) 
        => await _context.FeedingSchedules.Where(f => f.PetId == petId).ToListAsync(ct);
    public async Task AddAsync(FeedingSchedule schedule, CancellationToken ct = default)
    {
        await _context.FeedingSchedules.AddAsync(schedule, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(FeedingSchedule schedule, CancellationToken ct = default)
    {
        _context.FeedingSchedules.Update(schedule);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var schedule = await _context.FeedingSchedules.FirstOrDefaultAsync(f => f.Id == id, ct);
        if (schedule != null)
        {
            _context.FeedingSchedules.Remove(schedule);
            await _context.SaveChangesAsync(ct);
        }
    }
}

public class EFIncompatibilityRepository : IIncompatibilityRepository
{
    private readonly AppDbContext _context;
    public EFIncompatibilityRepository(AppDbContext context) => _context = context;

    public async Task<List<Incompatibility>> ListAsync(CancellationToken ct = default) 
        => await _context.Incompatibilities.Include(i => i.PetA).Include(i => i.PetB).ToListAsync(ct);
    public async Task<Incompatibility?> GetAsync(Guid id, CancellationToken ct = default) 
        => await _context.Incompatibilities.Include(i => i.PetA).Include(i => i.PetB).FirstOrDefaultAsync(i => i.Id == id, ct);
    public async Task AddAsync(Incompatibility incompatibility, CancellationToken ct = default)
    {
        await _context.Incompatibilities.AddAsync(incompatibility, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(Incompatibility incompatibility, CancellationToken ct = default)
    {
        _context.Incompatibilities.Update(incompatibility);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var incompatibility = await _context.Incompatibilities.FirstOrDefaultAsync(i => i.Id == id, ct);
        if (incompatibility != null)
        {
            _context.Incompatibilities.Remove(incompatibility);
            await _context.SaveChangesAsync(ct);
        }
    }
    public async Task<bool> ExistsAsync(Guid petAId, Guid petBId, CancellationToken ct = default)
    {
        return await _context.Incompatibilities.AnyAsync(i => 
            (i.PetAId == petAId && i.PetBId == petBId) || 
            (i.PetAId == petBId && i.PetBId == petAId), ct);
    }
}

public class EFPhotoUpdateRepository : IPhotoUpdateRepository
{
    private readonly AppDbContext _context;
    public EFPhotoUpdateRepository(AppDbContext context) => _context = context;

    public async Task<List<PhotoUpdate>> ListAsync(CancellationToken ct = default) 
        => await _context.PhotoUpdates.Include(p => p.Pet).ToListAsync(ct);
    public async Task<PhotoUpdate?> GetAsync(Guid id, CancellationToken ct = default) 
        => await _context.PhotoUpdates.Include(p => p.Pet).FirstOrDefaultAsync(p => p.Id == id, ct);
    public async Task<List<PhotoUpdate>> GetByPetIdAsync(Guid petId, CancellationToken ct = default) 
        => await _context.PhotoUpdates.Where(p => p.PetId == petId).OrderByDescending(p => p.Date).ToListAsync(ct);
    public async Task<int> GetTodayUpdateCountAsync(Guid petId, CancellationToken ct = default)
    {
        var today = DateTime.UtcNow.Date;
        return await _context.PhotoUpdates.CountAsync(p => p.PetId == petId && p.Date.Date == today, ct);
    }
    public async Task AddAsync(PhotoUpdate photoUpdate, CancellationToken ct = default)
    {
        await _context.PhotoUpdates.AddAsync(photoUpdate, ct);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdateAsync(PhotoUpdate photoUpdate, CancellationToken ct = default)
    {
        _context.PhotoUpdates.Update(photoUpdate);
        await _context.SaveChangesAsync(ct);
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var photoUpdate = await _context.PhotoUpdates.FirstOrDefaultAsync(p => p.Id == id, ct);
        if (photoUpdate != null)
        {
            _context.PhotoUpdates.Remove(photoUpdate);
            await _context.SaveChangesAsync(ct);
        }
    }
}
