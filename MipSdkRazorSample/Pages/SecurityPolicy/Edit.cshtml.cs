using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MipSdkRazorSample.Data;
using MipSdkRazorSample.Services;
using MipSdkRazorSample.Models;

namespace MipSdkRazorSample.Pages.SecurityPolicy
{
    public class EditModel : PageModel
    {
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;
        
        private readonly IMipService _mipApi;
        private readonly string _userId;

        public EditModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;

            _mipApi = _context.GetService<IMipService>();
            _userId = _context.GetService<IHttpContextAccessor>().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Subject.Name;
        }

        [BindProperty]
        public DataPolicy DataPolicy { get; set; }

        public SelectList MipLabelsSelectList { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DataPolicy = await _context.DataPolicy.FirstOrDefaultAsync(m => m.ID == id);
            MipLabelsSelectList = new SelectList(_mipApi.GetMipLabels(_userId), "Id", "Name", DataPolicy.MinLabelIdForAction);


            if (DataPolicy == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(DataPolicy).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DataPolicyExists(DataPolicy.ID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DataPolicyExists(int id)
        {
            return _context.DataPolicy.Any(e => e.ID == id);
        }
    }
}
