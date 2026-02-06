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
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<PetIncompatibilityGroup> IncompatibilitiesByPet { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? CurrentFilter { get; set; }

        public async Task OnGetAsync()
        {
            var incompatibilities = await _context.Incompatibilities
                .Include(i => i.PetA)
                .Include(i => i.PetB)
                .ToListAsync();

            // Create a list to hold all pet-incompatibility relationships
            var petIncompatibilities = new List<(Pet Pet, Domain.Entities.Incompatibility Incompatibility, Pet OtherPet)>();

            foreach (var incompatibility in incompatibilities)
            {
                if (incompatibility.PetA != null)
                {
                    petIncompatibilities.Add((incompatibility.PetA, incompatibility, incompatibility.PetB!));
                }
                if (incompatibility.PetB != null)
                {
                    petIncompatibilities.Add((incompatibility.PetB, incompatibility, incompatibility.PetA!));
                }
            }

            // Apply search filter if provided
            if (!string.IsNullOrEmpty(CurrentFilter))
            {
                var term = CurrentFilter.ToLower();
                petIncompatibilities = petIncompatibilities
                    .Where(x => x.Pet != null && x.Pet.Name.ToLower().Contains(term))
                    .ToList();
            }

            // Group by pet
            IncompatibilitiesByPet = petIncompatibilities
                .GroupBy(x => x.Pet)
                .Select(g => new PetIncompatibilityGroup
                {
                    Pet = g.Key,
                    Incompatibilities = g.Select(x => new IncompatibilityWithOtherPet
                    {
                        Id = x.Incompatibility.Id,
                        PetAId = x.Incompatibility.PetAId,
                        PetBId = x.Incompatibility.PetBId,
                        RuleDescription = x.Incompatibility.RuleDescription,
                        OtherPet = x.OtherPet
                    }).ToList()
                })
                .OrderBy(g => g.Pet?.Name)
                .ToList();
        }
    }

    public class PetIncompatibilityGroup
    {
        public Pet Pet { get; set; } = default!;
        public List<IncompatibilityWithOtherPet> Incompatibilities { get; set; } = new List<IncompatibilityWithOtherPet>();
    }

    public class IncompatibilityWithOtherPet : Domain.Entities.Incompatibility
    {
        public Pet? OtherPet { get; set; }
    }
}
