using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Entities;

namespace Eksam.Pages_Owners
{
    public class DeleteModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DeleteModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Owner Owner { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners.FirstOrDefaultAsync(m => m.Id == id);

            if (owner is not null)
            {
                Owner = owner;

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

            var owner = await _context.Owners.FindAsync(id);
            if (owner != null)
            {
                Owner = owner;
                _context.Owners.Remove(Owner);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
