using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.InformationProtection;
using MipSdkRazorSample.Models;
using MipSdkRazorSample.Services;
using System.Security.Claims;

namespace MipSdkRazorSample.Pages.DataImport
{
    public class IndexModel : PageModel
    {
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;
        private readonly IMipService _mipApi;
        private readonly string? _userId;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        public IndexModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;

            _mipApi = _context.GetService<IMipService>();
            _userId = _context.GetService<IHttpContextAccessor>().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Subject.Name;
        }

        public DataPolicy DataPolicy { get; set; }
        public IList<MipLabel> MipLabels { get; set; }  

        public async Task OnPostAsync()
        {
            MemoryStream uploadStream = new();

            DataPolicy = _context.DataPolicy.First(d => d.PolicyName == "Download Policy");

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
                        if(_mipApi.GetLabelSensitivityValue(label.Label.Id) <= _mipApi.GetLabelSensitivityValue(DataPolicy.MinLabelIdForAction))
                        {
                            // Do Processing
                        }
                        else
                        {
                            // Write Error
                        }
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
