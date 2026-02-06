using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Eksam.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
            Response.Redirect("/Dashboard");
        }
    }
}
