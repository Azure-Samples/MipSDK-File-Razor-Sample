using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MipSdkRazorSample.Data;
using MipSdkRazorSample.Models;

namespace MipSdkRazorSample.Pages.SecurityPolicy
{
    public class DeleteModel : PageModel
    {
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;

        public DeleteModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DataPolicy DataPolicy { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DataPolicy = await _context.DataPolicy.FirstOrDefaultAsync(m => m.ID == id);

            if (DataPolicy == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DataPolicy = await _context.DataPolicy.FindAsync(id);

            if (DataPolicy != null)
            {
                _context.DataPolicy.Remove(DataPolicy);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
