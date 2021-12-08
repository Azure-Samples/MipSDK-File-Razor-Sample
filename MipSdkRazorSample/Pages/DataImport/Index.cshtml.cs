using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.InformationProtection;
using MipSdkRazorSample.Models;
using MipSdkRazorSample.Services;
using System.Linq;
using System.Security.Claims;

namespace MipSdkRazorSample.Pages.DataImport
{
    public class IndexModel : PageModel
    {
        private readonly IExcelService _excelService;
        private readonly MipSdkRazorSample.Data.MipSdkRazorSampleContext _context;
        private readonly IMipService _mipApi;
        private readonly string? _userId;



        [BindProperty]
        public IFormFile? Upload { get; set; }

        public IndexModel(MipSdkRazorSample.Data.MipSdkRazorSampleContext context)
        {
            _context = context;

            _excelService = _context.GetService<IExcelService>();
            _mipApi = _context.GetService<IMipService>();
            _userId = _context.GetService<IHttpContextAccessor>().HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Subject.Name;
        }

        public DataPolicy DataPolicy { get; set; }

        public IList<Employee> Employees { get; set; }

        public IList<MipLabel> MipLabels { get; set; }  
        public string? Result { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            MemoryStream uploadStream = new();
                                    
            DataPolicy = _context.DataPolicy.First(d => d.PolicyName == "Download Policy");
            Employees = await _context.Employees.ToListAsync();

            if (Upload != null)
            {
                await Upload.CopyToAsync(uploadStream);
            }

            if (_mipApi != null && _userId != null)
            {
                uploadStream.Position = 0;

                ContentLabel label;

                try
                {
                    label = _mipApi.GetFileLabel(_userId, uploadStream);

                    if (_mipApi.IsLabeledOrProtected(uploadStream) == false)
                    {
                        Result = String.Format("Uploaded file isn't labeled.");

                        // Do Processing

                        var EmployeeUpload = _excelService.ParseUpload(uploadStream);

                        foreach (var employee in EmployeeUpload)
                        {
                            if (Employees.Any(e => e.ID == employee.ID))                            
                            {
                                var updatedEmployee = Employees.Where(e => e.ID == employee.ID).Single();
                                
                                updatedEmployee.FirstName = employee.FirstName;
                                updatedEmployee.Surname =  employee.Surname;
                                updatedEmployee.Salary = employee.Salary;
                                updatedEmployee.HireDate = employee.HireDate;
                                updatedEmployee.DateOfBirth = employee.DateOfBirth;
                                updatedEmployee.Title = employee.Title;
                                //_context.Attach(updatedEmployee).State = EntityState.Modified;
                                _context.Employees.Update(updatedEmployee);
                            }

                            else
                            {
                                var newEmployee = new Employee();
                                newEmployee.FirstName = employee.FirstName;
                                newEmployee.Surname = employee.Surname;
                                newEmployee.Salary = employee.Salary;
                                newEmployee.HireDate = employee.HireDate;
                                newEmployee.DateOfBirth = employee.DateOfBirth;
                                newEmployee.Title = employee.Title;

                                _context.Employees.Add(newEmployee);
                                //_context.Attach(employee).State = EntityState.Modified;
                            }                            
                        }
                        

                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            if (!EmployeesExists(Employees.FirstOrDefault().ID))
                            {
                                return NotFound();
                            }
                            else
                            {
                                throw;
                            }
                        }
                        Result = String.Format("Successfully to uploaded file with label: {0}", label.Label.Name);                        
                    }

                    else if (_mipApi.GetLabelSensitivityValue(label.Label.Id) <= _mipApi.GetLabelSensitivityValue(DataPolicy.MinLabelIdForAction))
                    {
                        // Do Processing


                        if (label.Label.Parent.Id == null)
                            Result = String.Format("Successfully to uploaded file with label: {0}", label.Label.Name);
                        else
                            Result = String.Format("Successfully to uploaded file with label: {0} - {1}", label.Label.Parent.Name, label.Label.Name);

                    }
                    else
                    {
                        // Write Error


                        if (label.Label.Parent.Id == null)
                            Result = String.Format("Failed to upload file. Service doesn't permit {0}", label.Label.Name);
                        else
                            Result = String.Format("Failed to upload file. Service doesn't permit {0} - {1}", label.Label.Parent.Name, label.Label.Name);
                    }
                }

                catch (Microsoft.InformationProtection.Exceptions.NoAuthTokenException)
                {

                }

                catch (Microsoft.InformationProtection.Exceptions.NoPermissionsException)
                {

                }

                catch (Microsoft.InformationProtection.Exceptions.AccessDeniedException)
                {
                    Result = "User doesn't have rights to uploaded file.";
                }                
            }
            return RedirectToPage("./Index");
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}