using Domain.Entities;

namespace Application.Abstractions;

public interface IOwnerRepository
{
    Task<List<Owner>> ListAsync(CancellationToken ct = default);
    Task<Owner?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<Owner>> SearchAsync(string searchTerm, CancellationToken ct = default);
    Task AddAsync(Owner owner, CancellationToken ct = default);
    Task UpdateAsync(Owner owner, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface IPetRepository
{
    Task<List<Pet>> ListAsync(CancellationToken ct = default);
    Task<Pet?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<Pet>> SearchAsync(string searchTerm, CancellationToken ct = default);
    Task AddAsync(Pet pet, CancellationToken ct = default);
    Task UpdateAsync(Pet pet, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface IKennelRepository
{
    Task<List<Kennel>> ListAsync(CancellationToken ct = default);
    Task<Kennel?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<Kennel>> SearchAsync(string searchTerm, CancellationToken ct = default);
    Task AddAsync(Kennel kennel, CancellationToken ct = default);
    Task UpdateAsync(Kennel kennel, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface IStayRepository
{
    Task<List<Stay>> ListAsync(CancellationToken ct = default);
    Task<Stay?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<Stay>> GetActiveStaysAsync(CancellationToken ct = default);
    Task AddAsync(Stay stay, CancellationToken ct = default);
    Task UpdateAsync(Stay stay, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface IMedicationRepository
{
    Task<List<Medication>> ListAsync(CancellationToken ct = default);
    Task<Medication?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<Medication>> GetByPetIdAsync(Guid petId, CancellationToken ct = default);
    Task AddAsync(Medication medication, CancellationToken ct = default);
    Task UpdateAsync(Medication medication, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface IMedicationLogRepository
{
    Task<List<MedicationLog>> ListAsync(CancellationToken ct = default);
    Task<MedicationLog?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<MedicationLog>> GetByMedicationIdAsync(Guid medicationId, CancellationToken ct = default);
    Task AddAsync(MedicationLog log, CancellationToken ct = default);
    Task UpdateAsync(MedicationLog log, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface IFeedingScheduleRepository
{
    Task<List<FeedingSchedule>> ListAsync(CancellationToken ct = default);
    Task<FeedingSchedule?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<FeedingSchedule>> GetByPetIdAsync(Guid petId, CancellationToken ct = default);
    Task AddAsync(FeedingSchedule schedule, CancellationToken ct = default);
    Task UpdateAsync(FeedingSchedule schedule, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface IIncompatibilityRepository
{
    Task<List<Incompatibility>> ListAsync(CancellationToken ct = default);
    Task<Incompatibility?> GetAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Incompatibility incompatibility, CancellationToken ct = default);
    Task UpdateAsync(Incompatibility incompatibility, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExistsAsync(Guid petAId, Guid petBId, CancellationToken ct = default);
}

public interface IPhotoUpdateRepository
{
    Task<List<PhotoUpdate>> ListAsync(CancellationToken ct = default);
    Task<PhotoUpdate?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<PhotoUpdate>> GetByPetIdAsync(Guid petId, CancellationToken ct = default);
    Task<int> GetTodayUpdateCountAsync(Guid petId, CancellationToken ct = default);
    Task AddAsync(PhotoUpdate photoUpdate, CancellationToken ct = default);
    Task UpdateAsync(PhotoUpdate photoUpdate, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}

public interface INotificationService
{
    Task SendDailyPhotoAsync(Owner owner, Pet pet, CancellationToken ct = default);
    Task SendLivestreamLinkAsync(Owner owner, Pet pet, CancellationToken ct = default);
    Task SendMissedCriticalMedicationAlertAsync(Owner owner, Pet pet, string medicationName, DateOnly date, TimeOnly time, CancellationToken ct = default);
}
