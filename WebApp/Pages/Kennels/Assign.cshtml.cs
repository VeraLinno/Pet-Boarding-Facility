using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Entities;
using Domain.Enums;
using Domain.Rules;

namespace Eksam.Pages_Kennels
{
    public class AssignModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public AssignModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid? SelectedPetId { get; set; }

        [BindProperty]
        public Guid? SelectedKennelId { get; set; }

        public Pet? SelectedPet { get; set; }
        public Kennel? SelectedKennel { get; set; }

        public SelectList AvailablePets { get; set; } = null!;
        public SelectList AvailableKennels { get; set; } = null!;

        public string ErrorMessage { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadDataAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (SelectedPetId == null || SelectedKennelId == null)
            {
                ErrorMessage = "Please select both a pet and a kennel.";
                return Page();
            }

            var pet = await _context.Pets.FindAsync(SelectedPetId.Value);
            var kennel = await _context.Kennels.FindAsync(SelectedKennelId.Value);

            if (pet == null || kennel == null)
            {
                ErrorMessage = "Pet or kennel not found.";
                return Page();
            }

            try            {
                KennelAssignmentRules.EnsureCanAssign(pet, kennel, DateTimeOffset.UtcNow);
            }
            catch (InvalidOperationException ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            // Assign the pet to the kennel
            pet.KennelId = kennel.Id;
            kennel.CurrentPetId = pet.Id;
            kennel.Status = KennelStatus.Occupied;

            _context.Pets.Update(pet);
            _context.Kennels.Update(kennel);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Kennels/Index");
        }

        private async Task LoadDataAsync()
        {
            // Load all pets without a kennel assignment
            var petsWithoutKennel = await _context.Pets
                .Where(p => p.KennelId == null)
                .Include(p => p.Owner)
                .OrderBy(p => p.Name)
                .ToListAsync();

            AvailablePets = new SelectList(petsWithoutKennel.Select(p => new
            {
                p.Id,
                DisplayName = $"{p.Name} ({p.Owner?.Email ?? "No owner"})"
            }), "Id", "DisplayName");

            // Load selected pet details
            if (SelectedPetId.HasValue)
            {
                SelectedPet = await _context.Pets
                    .Include(p => p.Owner)
                    .FirstOrDefaultAsync(p => p.Id == SelectedPetId.Value);

                if (SelectedPet != null)
                {
                    // Filter kennels based on pet requirements
                    var availableKennels = await GetFilteredKennelsAsync(SelectedPet);
                    AvailableKennels = new SelectList(availableKennels.Select(k => new
                    {
                        k.Id,
                        DisplayName = $"{k.Name} ({k.Size}, {k.Zone})"
                    }), "Id", "DisplayName");
                }
            }
            else
            {
                AvailableKennels = new SelectList(new List<object>());
            }

            // Load selected kennel details
            if (SelectedKennelId.HasValue)
            {
                SelectedKennel = await _context.Kennels.FindAsync(SelectedKennelId.Value);
            }
        }

        private async Task<List<Kennel>> GetFilteredKennelsAsync(Pet pet)
        {
            var now = DateTime.UtcNow;

            var kennels = await _context.Kennels
                .Where(k => k.Status == KennelStatus.Available)
                .Where(k => !k.CleaningUntilUtc.HasValue || k.CleaningUntilUtc.Value <= now)
                .ToListAsync();

            // Apply KennelAssignmentRules filters
            return kennels.Where(k =>
            {
                try                {
                    KennelAssignmentRules.EnsureCanAssign(pet, k, now);
                    return true;
                }
                catch
                {
                    return false;
                }
            }).ToList();
        }
    }
}
