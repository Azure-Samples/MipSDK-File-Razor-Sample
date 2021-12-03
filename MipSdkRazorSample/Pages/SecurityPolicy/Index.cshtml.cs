using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.InformationProtection;
using MipSdkRazorSample.Data;
using MipSdkRazorSample.MipApi;
using MipSdkRazorSample.Models;

namespace MipSdkRazorSample.Pages.SecurityPolicy
{
    public class IndexModel : PageModel
    {
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;

        private readonly IMipApi _mipApi;

        public IndexModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;
            
            _mipApi = context.GetService<IMipApi>();

            if (_mipApi != null)
            {
                ContentLabel label = _mipApi.GetFileLabel(new MemoryStream());
            }
        }

        public IList<DataPolicy> DataPolicy { get;set; }

        public async Task OnGetAsync()
        {
            DataPolicy = await _context.DataPolicy.ToListAsync();
        }
    }
}
