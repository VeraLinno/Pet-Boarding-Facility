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

namespace Eksam.Pages_Pets
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
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

            var pet =  await _context.Pets.FirstOrDefaultAsync(m => m.Id == id);
            if (pet == null)
            {
                return NotFound();
            }
            Pet = pet;
            ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Email");
            ViewData["KennelId"] = new SelectList(_context.Kennels, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ViewData["OwnerId"] = new SelectList(_context.Owners, "Id", "Email");
                ViewData["KennelId"] = new SelectList(_context.Kennels, "Id", "Name");
                return Page();
            }

            // Get the original pet to check if kennel changed
            var originalPet = await _context.Pets.AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == Pet.Id);
            
            var originalKennelId = originalPet?.KennelId;
            
            // Handle kennel assignment
            if (originalKennelId.HasValue && originalKennelId != Pet.KennelId)
            {
                // Clear the old kennel
                var oldKennel = await _context.Kennels.FindAsync(originalKennelId.Value);
                if (oldKennel != null)
                {
                    oldKennel.CurrentPetId = null;
                    oldKennel.Status = Domain.Enums.KennelStatus.Available;
                    _context.Kennels.Update(oldKennel);
                }
            }
            
            if (Pet.KennelId.HasValue)
            {
                // Assign to new kennel
                var newKennel = await _context.Kennels.FindAsync(Pet.KennelId.Value);
                if (newKennel != null)
                {
                    newKennel.CurrentPetId = Pet.Id;
                    newKennel.Status = Domain.Enums.KennelStatus.Occupied;
                    _context.Kennels.Update(newKennel);
                }
            }

            _context.Attach(Pet).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetExists(Pet.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PetExists(Guid id)
        {
            return _context.Pets.Any(e => e.Id == id);
        }
    }
}
