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
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<PetMedicationGroup> MedicationsByPet { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? CurrentFilter { get; set; }

        public async Task OnGetAsync()
        {
            var medications = await _context.Medications
                .Include(m => m.Pet)
                .Where(m => m.Pet != null)
                .ToListAsync();

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(CurrentFilter))
            {
                var term = CurrentFilter.ToLower();
                medications = medications
                    .Where(m => m.Pet != null && m.Pet.Name.ToLower().Contains(term))
                    .ToList();
            }

            // Client-side ordering (SQLite doesn't support TimeSpan ordering)
            medications = medications
                .OrderBy(m => m.Pet!.Name)
                .ThenBy(m => m.Time)
                .ToList();

            MedicationsByPet = medications
                .GroupBy(m => m.Pet!)
                .Select(g => new PetMedicationGroup
                {
                    Pet = g.Key!,
                    Medications = g.OrderBy(m => m.Time).ToList()
                })
                .ToList();
        }
    }

    public class PetMedicationGroup
    {
        public Pet Pet { get; set; } = default!;
        public List<Medication> Medications { get; set; } = new List<Medication>();
    }
}
