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

namespace Eksam.Pages_Incompatibility
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
            PetASelectList = new SelectList(_context.Pets, "Id", "Name");
            PetBSelectList = new SelectList(_context.Pets, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Incompatibility Incompatibility { get; set; } = default!;

        public SelectList PetASelectList { get; set; } = default!;
        public SelectList PetBSelectList { get; set; } = default!;

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PetASelectList = new SelectList(_context.Pets, "Id", "Name");
                PetBSelectList = new SelectList(_context.Pets, "Id", "Name");
                return Page();
            }

            // Validate that PetA and PetB are different
            if (Incompatibility.PetAId == Incompatibility.PetBId)
            {
                ErrorMessage = "Pet A and Pet B must be different pets.";
                PetASelectList = new SelectList(_context.Pets, "Id", "Name");
                PetBSelectList = new SelectList(_context.Pets, "Id", "Name");
                return Page();
            }

            // Check if incompatibility already exists (in either direction)
            var exists = await _context.Incompatibilities
                .AnyAsync(i => 
                    (i.PetAId == Incompatibility.PetAId && i.PetBId == Incompatibility.PetBId) ||
                    (i.PetAId == Incompatibility.PetBId && i.PetBId == Incompatibility.PetAId));

            if (exists)
            {
                ErrorMessage = "This incompatibility rule already exists.";
                PetASelectList = new SelectList(_context.Pets, "Id", "Name");
                PetBSelectList = new SelectList(_context.Pets, "Id", "Name");
                return Page();
            }

            Incompatibility.Id = Guid.NewGuid();
            _context.Incompatibilities.Add(Incompatibility);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
