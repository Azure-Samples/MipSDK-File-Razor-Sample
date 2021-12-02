using System.ComponentModel.DataAnnotations;

namespace MipSdkRazorSample.Models
{
    public class Employees
    {
        public int ID { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }

        public double Salary { get; set; }
    }
}
