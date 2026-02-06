using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Enums;

namespace Eksam.Pages_Kennels
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Kennel> Kennel { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? CurrentFilter { get; set; }

        public async Task OnGetAsync()
        {
            await RefreshKennelsAsync();
        }

        public async Task<IActionResult> OnPostStartCleaningAsync(Guid id)
        {
            var kennel = await _context.Kennels.FindAsync(id);
            if (kennel == null)
            {
                return NotFound();
            }

            kennel.Status = KennelStatus.Cleaning;
            kennel.CleaningStartedAtUtc = DateTime.UtcNow;
            kennel.CleaningUntilUtc = DateTime.UtcNow.AddHours(2);

            await _context.SaveChangesAsync();
            await RefreshKennelsAsync();
            
            return Page();
        }

        public async Task<IActionResult> OnPostCompleteCleaningAsync(Guid id)
        {
            var kennel = await _context.Kennels.FindAsync(id);
            if (kennel == null)
            {
                return NotFound();
            }

            kennel.Status = KennelStatus.Available;
            kennel.CleaningStartedAtUtc = null;
            kennel.CleaningUntilUtc = null;

            await _context.SaveChangesAsync();
            await RefreshKennelsAsync();
            
            return Page();
        }

        private async Task RefreshKennelsAsync()
        {
            IQueryable<Kennel> kennelsIQ = _context.Kennels.Include(k => k.CurrentPet);

            if (!string.IsNullOrEmpty(CurrentFilter))
            {
                var term = CurrentFilter.ToLower();
                kennelsIQ = kennelsIQ.Where(k => k.Name.ToLower().Contains(term));
            }

            Kennel = await kennelsIQ.AsNoTracking().ToListAsync();
        }
    }
}
