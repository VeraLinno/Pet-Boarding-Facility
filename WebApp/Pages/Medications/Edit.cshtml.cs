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

namespace Eksam.Pages_Medications
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Medication Medication { get; set; } = default!;

        public SelectList PetSelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications
                .Include(m => m.Pet)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medication == null)
            {
                return NotFound();
            }

            Medication = medication;
            PetSelectList = new SelectList(_context.Pets, "Id", "Name", Medication.PetId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PetSelectList = new SelectList(_context.Pets, "Id", "Name", Medication.PetId);
                return Page();
            }

            var medicationToUpdate = await _context.Medications.FindAsync(Medication.Id);
            if (medicationToUpdate == null)
            {
                return NotFound();
            }

            medicationToUpdate.PetId = Medication.PetId;
            medicationToUpdate.Name = Medication.Name;
            medicationToUpdate.Time = Medication.Time;
            medicationToUpdate.SpecialCondition = Medication.SpecialCondition;
            medicationToUpdate.Required = Medication.Required;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicationExists(Medication.Id))
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

        private bool MedicationExists(Guid id)
        {
            return _context.Medications.Any(e => e.Id == id);
        }
    }
}
