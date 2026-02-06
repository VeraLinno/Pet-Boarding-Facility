using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;
using DAL;

namespace Eksam.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly AppDbContext _context;

        public DashboardModel(AppDbContext context)
        {
            _context = context;
        }

        public List<Pet> PetsBoarding { get; set; } = new();
        public List<Medication> MedicationsDueToday { get; set; } = new();
        public List<PetPhotoStatus> PetsPhotoStatus { get; set; } = new();

        public int AvailableKennels { get; set; }
        public int OccupiedKennels { get; set; }

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);

            // Kennel counts
            AvailableKennels = await _context.Kennels.CountAsync(k => k.Status == KennelStatus.Available);
            OccupiedKennels = await _context.Kennels.CountAsync(k => k.Status == KennelStatus.Occupied);

            // Pets boarding
            PetsBoarding = await _context.Kennels
                .Where(k => k.Status == KennelStatus.Occupied && k.CurrentPet != null)
                .Include(k => k.CurrentPet!)
                    .ThenInclude(p => p.Owner)
                .Include(k => k.CurrentPet!)
                    .ThenInclude(p => p.Medications)
                .Include(k => k.CurrentPet!)
                    .ThenInclude(p => p.Kennel)
                .Select(k => k.CurrentPet!)
                .OrderBy(p => p.Name)
                .ToListAsync();

            var petIds = PetsBoarding.Select(p => p.Id).ToList();

            // **Medications due today, excluding already given**
            var medicationLogsToday = await _context.MedicationLogs
                .Where(l => l.Date == today && l.Given)
                .Select(l => l.MedicationId)
                .ToListAsync();

            MedicationsDueToday = await _context.Medications
                .Include(m => m.Pet)
                .Where(m => petIds.Contains(m.PetId))
                .Where(m => !medicationLogsToday.Contains(m.Id))
                .ToListAsync();

            MedicationsDueToday = MedicationsDueToday.OrderBy(m => m.Time).ToList();

            // Photo status - count actual photos per pet and determine requirements based on premium level
            var photoCountsToday = await _context.PhotoUpdates
                .Where(p => p.Date.Date == DateTime.Today)
                .GroupBy(p => p.PetId)
                .ToDictionaryAsync(g => g.Key, g => g.Count());

            PetsPhotoStatus = PetsBoarding
                .Select(p =>
                {
                    var photosSent = photoCountsToday.TryGetValue(p.Id, out var count) ? count : 0;
                    var isPremium = p.Owner?.PremiumLevel == PremiumLevel.Premium;
                    var photosRequired = isPremium ? 2 : 1;
                    var needsMorePhotos = photosSent < photosRequired;
                    
                    return new PetPhotoStatus
                    {
                        Pet = p,
                        PhotoSentToday = photosSent > 0,
                        PhotosSentToday = photosSent,
                        PhotosRequiredToday = photosRequired,
                        NeedsPhoto = needsMorePhotos
                    };
                })
                .ToList();
        }

        public async Task<IActionResult> OnPostMarkMedicationGivenAsync(Guid medicationId)
        {
            var medication = await _context.Medications
                .Include(m => m.Pet)
                .FirstOrDefaultAsync(m => m.Id == medicationId);

            if (medication == null)
            {
                return NotFound();
            }

            var log = new MedicationLog
            {
                Id = Guid.NewGuid(),
                MedicationId = medication.Id,
                Date = DateOnly.FromDateTime(DateTime.Today),
                ScheduledTime = medication.Time,
                Given = true,
                GivenAt = DateTime.Now,
                StaffInitials = "STAFF"
            };

            _context.MedicationLogs.Add(log);
            await _context.SaveChangesAsync();

            await LoadDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostMarkPhotoSentAsync(Guid petId)
        {
            var pet = await _context.Pets.FindAsync(petId);
            if (pet == null)
            {
                return NotFound();
            }

            var photoUpdate = new PhotoUpdate
            {
                Id = Guid.NewGuid(),
                PetId = petId,
                Date = DateTime.Now,
                ImagePath = "/uploads/placeholder.jpg",
                SentToOwner = true,
                UpdateType = UpdateType.Daily
            };

            _context.PhotoUpdates.Add(photoUpdate);
            await _context.SaveChangesAsync();

            await LoadDataAsync();
            return Page();
        }
    }

    public class PetPhotoStatus
    {
        public Pet Pet { get; set; } = null!;
        public bool PhotoSentToday { get; set; }
        public int PhotosSentToday { get; set; }
        public int PhotosRequiredToday { get; set; }
        public bool NeedsPhoto { get; set; }
    }
}
