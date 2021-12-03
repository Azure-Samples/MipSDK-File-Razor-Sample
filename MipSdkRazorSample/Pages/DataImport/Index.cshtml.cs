using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MipSdkRazorSample.Pages.DataImport
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public IFormFile? Upload { get; set; }

        public async Task OnPostAsync()
        {
            MemoryStream uploadStream = new();

            if (Upload != null)
            {
                await Upload.CopyToAsync(uploadStream);
            }

            // Process File
        }
    }
}
