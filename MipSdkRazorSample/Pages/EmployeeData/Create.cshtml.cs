using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MipSdkRazorSample.Data;
using MipSdkRazorSample.Models;

namespace MipSdkRazorSample.Pages.EmployeeData
{
    public class CreateModel : PageModel
    {
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;

        public CreateModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Employee Employees { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Employees.Add(Employees);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
