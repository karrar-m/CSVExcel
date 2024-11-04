using CSVExcel.Interface;
using CSVExcel.Model.Employee;
using OfficeOpenXml;
using System.Globalization;

namespace CSVExcel.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeValidator _validation;

        public EmployeeService(EmployeeValidator validations)
        {
            _validation = validations; 

        }

        public async Task<bool> ExcelEmployee(IFormFile file)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            if (file == null || file.Length == 0)
                throw new ArgumentNullException(nameof(file), "File is invalid");

            var employees = await ProcessEmployeeFile(file);

            if (employees != null && employees.Count > 0) // تحقق من وجود موظفين
            {
                await _validation.ValidateEmployeesAsync(employees);

                // await _dbContext.Employees.AddRangeAsync(employees);
                // await _dbContext.SaveChangesAsync();

                return true;     
            }

            return false; 
        }

        private async Task<List<Employee>> ProcessEmployeeFile(IFormFile file)
        {
            var employees = new List<Employee>();

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];
            int rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var employee = ParseEmployeeFromRow(worksheet, row);
                if (employee != null)
                {
                    employees.Add(employee);
                }
            }
            return employees;
        }

        private Employee ParseEmployeeFromRow(ExcelWorksheet worksheet, int row)
        {
            return new Employee
            {
                FirstName = worksheet.Cells[row, 1].Text.Trim(),
                SecondName = worksheet.Cells[row, 2].Text.Trim(),
                ThirdName = worksheet.Cells[row, 3].Text.Trim(),
                ForthName = worksheet.Cells[row, 4].Text.Trim(),
                LastName = worksheet.Cells[row, 5].Text.Trim(),
                MotherName = worksheet.Cells[row, 6].Text.Trim(),
                Gender = worksheet.Cells[row, 7].Text.Trim(),
                DateOfBirth = ParseDate(worksheet.Cells[row, 8].Text) ?? DateTime.MinValue,
                Nationality = worksheet.Cells[row, 9].Text.Trim(),
                JobService = worksheet.Cells[row, 10].Text.Trim(),
                Education = worksheet.Cells[row, 11].Text.Trim(),
                Country = worksheet.Cells[row, 12].Text.Trim(),
                Address = worksheet.Cells[row, 13].Text.Trim(),
                HiringDate = ParseDate(worksheet.Cells[row, 14].Text) ?? DateTime.MinValue,
                HiringType = worksheet.Cells[row, 15].Text.Trim(),
                WorkplaceAddress = worksheet.Cells[row, 16].Text.Trim(),
                EmployeeNumber = worksheet.Cells[row, 17].Text.Trim(),
                MonthlySalary = TryParseDecimal(worksheet.Cells[row, 18].Text),
                Allowances = TryParseDecimal(worksheet.Cells[row, 19].Text),
            };
        }
        private decimal TryParseDecimal(string decimalText)
        {
            return decimal.TryParse(decimalText, out var parsedDecimal) ? parsedDecimal : 0;
        }

        private DateTime? ParseDate(string dateText)
        {
            if (DateTime.TryParse(dateText, out var date))
            {
                return date;
            }
            return null;
        }



    }
}

