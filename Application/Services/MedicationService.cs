using Application.Abstractions;
using Domain.Entities;

namespace Application.Services;

public sealed class MedicationService
{
    private readonly IMedicationRepository _medications;
    private readonly IMedicationLogRepository _medicationLogs;
    private readonly IPetRepository _pets;

    public MedicationService(
        IMedicationRepository medications,
        IMedicationLogRepository medicationLogs,
        IPetRepository pets)
    {
        _medications = medications;
        _medicationLogs = medicationLogs;
        _pets = pets;
    }

    public async Task<List<Medication>> GetTodayMedicationsAsync(DateOnly date, CancellationToken ct = default)
    {
        var medications = await _medications.ListAsync(ct);
        
        // Get medications for pets with active stays
        var pets = await _pets.ListAsync(ct);
        var activePetIds = pets.Select(p => p.Id).ToHashSet();
        
        return medications.Where(m => activePetIds.Contains(m.PetId)).ToList();
    }

    public async Task<MedicationLog> RecordMedicationGivenAsync(
        Guid medicationId, 
        string staffInitials, 
        CancellationToken ct = default)
    {
        var medication = await _medications.GetAsync(medicationId, ct) 
            ?? throw new InvalidOperationException("Medication not found.");

        var log = new MedicationLog
        {
            MedicationId = medicationId,
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            ScheduledTime = medication.Time,
            Given = true,
            GivenAt = DateTime.UtcNow,
            StaffInitials = staffInitials
        };

        await _medicationLogs.AddAsync(log, ct);
        return log;
    }

    public async Task<List<MedicationLog>> GetMissedMedicationsAsync(DateOnly date, CancellationToken ct = default)
    {
        var logs = await _medicationLogs.ListAsync(ct);
        var medications = await _medications.ListAsync(ct);
        var medicationIds = medications.Select(m => m.Id).ToHashSet();

        // Get logs for today that are not marked as given
        var todayLogs = logs.Where(l => 
            l.Date == date && 
            !l.Given &&
            medicationIds.Contains(l.MedicationId)).ToList();

        return todayLogs;
    }

    public async Task GenerateDailyMedicationLogsAsync(DateOnly date, CancellationToken ct = default)
    {
        var medications = await GetTodayMedicationsAsync(date, ct);
        var existingLogs = await _medicationLogs.ListAsync(ct);

        foreach (var medication in medications)
        {
            // Check if log already exists
            var exists = existingLogs.Any(l => 
                l.MedicationId == medication.Id && 
                l.Date == date);

            if (!exists)
            {
                var log = new MedicationLog
                {
                    MedicationId = medication.Id,
                    Date = date,
                    ScheduledTime = medication.Time,
                    Given = false,
                    GivenAt = null,
                    StaffInitials = string.Empty
                };

                await _medicationLogs.AddAsync(log, ct);
            }
        }
    }
}
