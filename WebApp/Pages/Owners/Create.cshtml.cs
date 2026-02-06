using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL;
using Domain.Entities;
using Domain.Enums;

namespace Eksam.Pages_Owners
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Owner Owner { get; set; } = default!;
        
        [BindProperty]
        public bool IsPremium { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Owner.PremiumLevel = IsPremium
                ? PremiumLevel.Premium
                : PremiumLevel.Standard;

            _context.Owners.Add(Owner);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}