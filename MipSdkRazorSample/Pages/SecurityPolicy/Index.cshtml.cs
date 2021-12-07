using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.InformationProtection;
using MipSdkRazorSample.Data;
using MipSdkRazorSample.Services; 
using MipSdkRazorSample.Models;

namespace MipSdkRazorSample.Pages.SecurityPolicy
{
    public class IndexModel : PageModel
    {
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;
        private readonly IMipService _mipApi;
        private readonly string _userId;

        

        public IndexModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {                                
            _context = context;
            
            _mipApi = _context.GetService<IMipService>();
            _userId = _context.GetService<IHttpContextAccessor>().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Subject.Name;                                                      
        }

        public IList<DataPolicy> DataPolicy { get;set; }
        public IList<MipLabel> MipLabel { get; set; }

        public async Task OnGetAsync()
        {

            MipLabel =  _mipApi.GetMipLabels(_userId);
            DataPolicy = await _context.DataPolicy.ToListAsync();
        }
    }
}
