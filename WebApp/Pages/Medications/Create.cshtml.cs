using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain.Entities;

namespace Eksam.Pages_Medications
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PetSelectList = new SelectList(_context.Pets, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Medication Medication { get; set; } = default!;

        public SelectList PetSelectList { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PetSelectList = new SelectList(_context.Pets, "Id", "Name");
                return Page();
            }

            Medication.Id = Guid.NewGuid();
            _context.Medications.Add(Medication);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
