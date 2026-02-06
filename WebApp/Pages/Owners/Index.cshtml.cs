using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain.Entities;

namespace Eksam.Pages_Owners
{
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<Owner> Owner { get;set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string? CurrentFilter { get; set; }

        public string NameSort { get; set; } = string.Empty;
        public string EmailSort { get; set; } = string.Empty;

        public async Task OnGetAsync(string sortOrder)
        {
            NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            EmailSort = sortOrder == "Email" ? "email_desc" : "Email";

            IQueryable<Owner> ownersIQ = _context.Owners;

            if (!string.IsNullOrEmpty(CurrentFilter))
            {
                var term = CurrentFilter.ToLower();
                ownersIQ = ownersIQ.Where(o =>
                    o.Name.ToLower().Contains(term) ||
                    o.Email.ToLower().Contains(term));
            }

            Owner = await ownersIQ.AsNoTracking().ToListAsync();
        }
    }
}
