using Microsoft.EntityFrameworkCore;
using MipSdkRazorSample.Data;

namespace MipSdkRazorSample.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MipSdkRazorSampleContext(serviceProvider.GetRequiredService<DbContextOptions<MipSdkRazorSampleContext>>()))
            {
                if (context == null || context.DataPolicy == null)
                {
                    throw new ArgumentNullException("Null MipSdkRazorSampleContext");
                }                                                                    

                if (!context.DataPolicy.Any())
                {
                    context.DataPolicy.AddRange(
                    new DataPolicy
                    {
                        Direction = DataPolicy.PolicyDirection.Download,
                        PolicyName = "Download Policy",
                        MinLabelIdForAction = "000"
                    },
                    new DataPolicy
                    {
                        Direction = DataPolicy.PolicyDirection.Upload,
                        PolicyName = "Upload Policy",
                        MinLabelIdForAction = "000"
                    });
                    context.SaveChanges();
                }

                else if(!context.Employees.Any())
                {
                    context.Employees.AddRange(
                        new Employee
                        {
                            FirstName = "Megan",
                            Surname = "Bowen",
                            DateOfBirth = new DateTime(1979, 06, 23),
                            HireDate = new DateTime(1999, 01,01),
                            Salary = 250000, 
                            Title = "Chief Cryptographer"
                        },
                        new Employee
                        {
                            FirstName = "Miriam",
                            Surname = "Graham", 
                            DateOfBirth = new DateTime(1968, 5, 16),
                            HireDate = new DateTime(1992, 02, 27), 
                            Salary = 250000, 
                            Title = "Chief Systems Programmer"
                        },
                        new Employee
                        {
                            FirstName = "Raul",
                            Surname = "Razo",
                            DateOfBirth = new DateTime(1958, 10, 31),
                            HireDate = new DateTime(1991, 04, 01),
                            Salary = 250000,
                            Title = "Vice President - Customer Experience"
                        },
                        new Employee
                        {
                            FirstName = "Nestor",
                            Surname = "Wilke",
                            DateOfBirth = new DateTime(1991, 3, 09),
                            HireDate = new DateTime(2011, 06 , 05),
                            Salary = 250000,
                            Title = "Consultant"
                        },
                        new Employee
                        {
                            FirstName = "Lee",
                            Surname = "Gu",
                            DateOfBirth = new DateTime(2001, 07, 04),
                            HireDate = new DateTime(2020, 07, 10),
                            Salary = 250000,
                            Title = "Intern"
                        }
                    ) ;
                    context.SaveChanges();
                }

                else
                {
                    return;
                }                                
            }
        }
    }
}
