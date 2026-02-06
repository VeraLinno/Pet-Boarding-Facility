using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Entities;

namespace Eksam.Pages_Medications
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Medication Medication { get; set; } = default!;

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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Medication.Id == Guid.Empty)
            {
                return NotFound();
            }

            var medication = await _context.Medications.FindAsync(Medication.Id);
            if (medication != null)
            {
                _context.Medications.Remove(medication);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
