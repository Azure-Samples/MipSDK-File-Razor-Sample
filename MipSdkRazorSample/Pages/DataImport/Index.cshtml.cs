using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.InformationProtection;
using MipSdkRazorSample.MipApi;
using System.Security.Claims;

namespace MipSdkRazorSample.Pages.DataImport
{
    public class IndexModel : PageModel
    {
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;
        private readonly IMipApi _mipApi;
        private readonly string? _userId;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public IndexModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;

            _mipApi = _context.GetService<IMipApi>();
            _userId = _context.GetService<IHttpContextAccessor>().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Subject.Name;
        }

        
        public async Task OnPostAsync()
        {
            MemoryStream uploadStream = new();

            if (Upload != null)
            {
                await Upload.CopyToAsync(uploadStream);
            }
           
            if (_mipApi != null)
            {
                uploadStream.Position = 0;
                
                if (_mipApi != null)
                {
                    if (_userId != null)
                    {
                        ContentLabel label = _mipApi.GetFileLabel(_userId, uploadStream);
                        
                    }
                    else
                    {
                        throw new Exception("No UserId Specified");
                    }
                }
            }

            // Process File
        }
    }
}
