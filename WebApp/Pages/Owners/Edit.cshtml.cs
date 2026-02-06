using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Entities;
using Domain.Enums;

namespace Eksam.Pages_Owners
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Owner Owner { get; set; } = default!;

        // 🔴 BUG OLI SIIN → nüüd parandatud
        [BindProperty]
        public bool IsPremium { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var owner = await _context.Owners.FirstOrDefaultAsync(m => m.Id == id);
            if (owner == null)
            {
                return NotFound();
            }

            Owner = owner;
            IsPremium = owner.PremiumLevel == PremiumLevel.Premium;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Owner.PremiumLevel = IsPremium
                ? PremiumLevel.Premium
                : PremiumLevel.Standard;

            _context.Attach(Owner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OwnerExists(Owner.Id))
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

        private bool OwnerExists(Guid id)
        {
            return _context.Owners.Any(e => e.Id == id);
        }
    }
}
