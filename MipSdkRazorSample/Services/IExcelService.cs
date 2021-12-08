using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.InformationProtection;
using Microsoft.InformationProtection.Exceptions;
using Microsoft.InformationProtection.File;
using MipSdkRazorSample.Models;

namespace MipSdkRazorSample.Services
{
    public interface IExcelService
    {
        public Stream GenerateExcelFile(DataTable table);
        
        public Stream GenerateEmployeeExport(IEnumerable<Employee> employees);

        public List<Employee> ParseUpload(Stream upload);
        
    }
}
