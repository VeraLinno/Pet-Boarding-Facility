using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Entities;

namespace Eksam.Pages_Incompatibility
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Incompatibility Incompatibility { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incompatibility = await _context.Incompatibilities
                .Include(i => i.PetA)
                .Include(i => i.PetB)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (incompatibility == null)
            {
                return NotFound();
            }

            Incompatibility = incompatibility;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Incompatibility.Id == Guid.Empty)
            {
                return NotFound();
            }

            var incompatibility = await _context.Incompatibilities.FindAsync(Incompatibility.Id);
            if (incompatibility != null)
            {
                _context.Incompatibilities.Remove(incompatibility);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
