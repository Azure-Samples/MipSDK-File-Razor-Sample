using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.Extensions.Configuration;
using Microsoft.InformationProtection;
using Microsoft.InformationProtection.Exceptions;
using Microsoft.InformationProtection.File;
using MipSdkRazorSample.Models;
using System.Data;

namespace MipSdkRazorSample.Services
{
    public class ExcelService : IExcelService
    {
        public Stream GenerateEmployeeExport(IEnumerable<Employee> employees)
        {
            DataTable dt = new DataTable("EmployeeData");
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

            foreach (var employee in employees)
            {
                dt.Rows.Add(employee.ID, employee.FirstName, employee.Surname, employee.Title, employee.DateOfBirth, employee.HireDate, employee.Salary);
            }

            return GenerateExcelFile(dt);
        }

        public Stream GenerateExcelFile(DataTable table)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                MemoryStream stream = new MemoryStream();
                wb.Worksheets.Add(table);
                wb.SaveAs(stream);
                return stream;
            }
        }

        public List<Employee> ParseUpload(Stream upload)
        {
            List<Employee> employees = new List<Employee>();
            using (var wb = new XLWorkbook(upload))
            {
                var ws = wb.Worksheet(1);
                ws.Name = "EmployeeData";
                var firstCell = ws.FirstCellUsed();
                var lastCell = ws.LastCellUsed();

                var range = ws.Range(firstCell.Address, lastCell.CellRight().Address);
                range.FirstRow().Delete();

                var table = ws.Tables.Table("EmployeeData");

                foreach (var row in table.Rows())
                {                    
                    employees.Add(new Employee()
                    {
                        ID = Convert.ToInt32(row.Cell(1).GetDouble()),
                        FirstName = row.Cell(2).GetString(),
                        Surname = row.Cell(3).GetString(),
                        Title = row.Cell(4).GetString(),
                        DateOfBirth = row.Cell(5).GetDateTime(),
                        HireDate = row.Cell(6).GetDateTime(),
                        Salary = row.Cell(7).GetDouble()
                    });
                }
            }

            return employees;
        }
    }
}
