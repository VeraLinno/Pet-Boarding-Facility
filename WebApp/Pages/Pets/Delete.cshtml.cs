using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Entities;
using Domain.Enums;

namespace Eksam.Pages_Pets
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Pet Pet { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FirstOrDefaultAsync(m => m.Id == id);

            if (pet is not null)
            {
                Pet = pet;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Stays)
                .Include(p => p.Medications)
                .ThenInclude(m => m.Logs)
                .Include(p => p.FeedingSchedules)
                .Include(p => p.PhotoUpdates)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (pet == null)
            {
                return NotFound();
            }

            // Delete all related entities first
            // Delete MedicationLogs first (dependent on Medications)
            foreach (var medication in pet.Medications)
            {
                var logs = await _context.MedicationLogs
                    .Where(l => l.MedicationId == medication.Id)
                    .ToListAsync();
                _context.MedicationLogs.RemoveRange(logs);
            }
            
            // Delete Medications
            _context.Medications.RemoveRange(pet.Medications);
            
            // Delete FeedingSchedules
            _context.FeedingSchedules.RemoveRange(pet.FeedingSchedules);
            
            // Delete Stays
            _context.Stays.RemoveRange(pet.Stays);
            
            // Delete PhotoUpdates
            _context.PhotoUpdates.RemoveRange(pet.PhotoUpdates);
            
            // Remove pet from any Incompatibility records
            var incompatibilities = await _context.Incompatibilities
                .Where(i => i.PetAId == pet.Id || i.PetBId == pet.Id)
                .ToListAsync();
            _context.Incompatibilities.RemoveRange(incompatibilities);

            // Update the Kennel: clear CurrentPet and set status to Available
            if (pet.KennelId.HasValue)
            {
                var kennel = await _context.Kennels.FindAsync(pet.KennelId);
                if (kennel != null)
                {
                    kennel.CurrentPetId = null;
                    kennel.Status = KennelStatus.Available;
                }
            }

            // Clear navigation collections to avoid EF Core tracking issues
            pet.Medications.Clear();
            pet.Stays.Clear();
            pet.FeedingSchedules.Clear();
            pet.PhotoUpdates.Clear();
            
            _context.Pets.Remove(pet);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
