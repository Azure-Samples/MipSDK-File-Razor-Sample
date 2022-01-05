using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MipSdkRazorSample.Data;
using MipSdkRazorSample.Models;

namespace MipSdkRazorSample.Pages.EmployeeData
{
    public class DetailsModel : PageModel
    {
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;

        public DetailsModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;
        }

        public Employee Employees { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employees = await _context.Employees.FirstOrDefaultAsync(m => m.ID == id);

            if (Employees == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
