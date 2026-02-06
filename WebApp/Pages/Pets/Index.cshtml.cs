using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Eksam.Pages_Pets
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Pet> Pet { get;set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        public async Task OnGetAsync()
        {
            var pets = _context.Pets.Include(p => p.Owner).AsQueryable();
            
            if (!string.IsNullOrEmpty(SearchString))
            {
                var term = SearchString.ToLower();
                pets = pets.Where(p => 
                    p.Name.ToLower().Contains(term) || 
                    (p.Owner != null && p.Owner.Email.ToLower().Contains(term)));
            }
            
            Pet = await pets.ToListAsync();
        }
    }
}
