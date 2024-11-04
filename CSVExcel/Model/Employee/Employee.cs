namespace CSVExcel.Model.Employee;

using System;

public class Employee
{
    public string FirstName { get; set; }
    public string SecondName { get; set; }
    public string ThirdName { get; set; }
    public string ForthName { get; set; }
    public string LastName { get; set; }
    public string MotherName { get; set; }
    public string Gender { get; set; } // "ذكر" or "أنثى"
    public DateTime DateOfBirth { get; set; }
    public string Nationality { get; set; } // "عربي" or "أجنبي" or "عراقي"
    public string JobService { get; set; } // Type of job
    public string Education { get; set; } // "أمي" to "دكتوراه"
    public string Country { get; set; }
    public string Address { get; set; }
    public DateTime HiringDate { get; set; }
    public string HiringType { get; set; }

    public string WorkplaceAddress { get; set; }
    public string EmployeeNumber { get; set; } // Should not exceed 15 digits
    public decimal MonthlySalary { get; set; } // Should not be less than 266 or 350000 IQD
    public decimal Allowances { get; set; } // Should be at least 0
}
