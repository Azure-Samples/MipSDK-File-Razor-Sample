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
        private readonly IExcelService _excelService;

        public IndexModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;

            _excelService = _context.GetService<IExcelService>(); 
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
            string labelId = _context.DataPolicy.First(d => d.PolicyName == "Download Policy").MinLabelIdForAction;

            var excelStream = _excelService.GenerateEmployeeExport(_context.Employees.ToList());
            MemoryStream? mipStream = _mipApi.ApplyMipLabel(excelStream, labelId);
            mipStream.Position = 0;

            return File(mipStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "EmployeeData.xlsx");

        }
    }
}
