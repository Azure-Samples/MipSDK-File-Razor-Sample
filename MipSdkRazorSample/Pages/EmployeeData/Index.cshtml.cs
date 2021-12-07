using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MipSdkRazorSample.Data;
using MipSdkRazorSample.Models;
using MipSdkRazorSample.Services;


namespace MipSdkRazorSample.Pages.EmployeeData
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

        public IList<Employee> Employees { get;set; }
        public DataPolicy DataPolicy { get; set; }
        
        public IList<MipLabel> MipLabel { get; set; }


        public async Task OnGetAsync()
        {
            MipLabel = _mipApi.GetMipLabels(_userId);
            DataPolicy = _context.DataPolicy.First(d => d.PolicyName == "Download Policy");
            Employees = await _context.Employees.ToListAsync();
        }

        // Referenced this: https://www.aspsnippets.com/Articles/ASPNet-Core-Razor-Pages-Export-to-Excel.aspx
        public FileResult OnPostExport()
        {
            Employees = _context.Employees.ToList();
            string labelId = _context.DataPolicy.First(d => d.PolicyName == "Download Policy").MinLabelIdForAction;

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[7]
            {
                new DataColumn("ID"),
                new DataColumn("FirstName"),
                new DataColumn("Surname"),
                new DataColumn("Title"),
                new DataColumn("DateOfBirth"),
                new DataColumn("HireDate"),
                new DataColumn("Salary")                
            });
            
            foreach (var employee in Employees)
            {
                dt.Rows.Add(employee.ID, employee.FirstName, employee.Surname, employee.Title, employee.DateOfBirth, employee.HireDate, employee.Salary);
            }
            
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    // Send stream to IMipService for labeling.
                    // Label will be applied with service as owner.                        

                    return File(_mipApi.ApplyMipLabel(stream, labelId).ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeData.xlsx");
                }
                        
            }

        }
    }
}
