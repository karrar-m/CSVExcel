using ClosedXML.Excel;
using CSVExcel.Exceptions;
using CSVExcel.Interface;
using CSVExcel.Model.Employee;

namespace CSVExcel.Service
{
    public class EmployeeService : IEmployeeService
    {
        private readonly EmployeeValidator _validation;

        public EmployeeService(EmployeeValidator validations)
        {
            _validation = validations; 

        }

        public async Task<bool> ExcelEmployee(IFormFile file, CancellationToken cancellationToken)
        {
                ValidateFile(file); 
            var employees = await ProcessEmployeeFile(file, cancellationToken);

            if (employees != null && employees.Count > 0) 
            {
                await _validation.ValidateEmployeesAsync(employees, cancellationToken);

               // await _dbContext.Employees.AddRangeAsync(employees);
                // await _dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }


        private async Task<List<Employee>> ProcessEmployeeFile(IFormFile file, CancellationToken cancellationToken)
        {

            var employees = new List<Employee>();
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            stream.Position = 0; 
            using var workbook = new XLWorkbook(stream);
            var worksheet = workbook.Worksheet(1);
            employees = await ParseAddEmployeeAsync (worksheet , cancellationToken);   
            return employees;
        }

        private Task<List<Employee>> ParseAddEmployeeAsync(IXLWorksheet worksheet, CancellationToken cancellationToken)
        {
            var employees = new List<Employee>();
            int? rowCount = worksheet.LastRowUsed()?.RowNumber();
            cancellationToken.ThrowIfCancellationRequested();

            for (int row = 2; row <= rowCount; row++)
            {
                var employee = ParseEmployeeFromRow(worksheet, row, cancellationToken);
                if (employee != null)
                {
                    employees.Add(employee);
                }
            }
            return Task.FromResult(employees);
        }
        private Employee ParseEmployeeFromRow(IXLWorksheet worksheet, int row ,CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return new Employee
            {
                FirstName = worksheet.Cell(row, 1).GetString().Trim(),
                SecondName = worksheet.Cell(row, 2).GetString().Trim(),
                ThirdName = worksheet.Cell(row, 3).GetString().Trim(),
                ForthName = worksheet.Cell(row, 4).GetString().Trim(),
                LastName = worksheet.Cell(row, 5).GetString().Trim(),
                MotherName = worksheet.Cell(row, 6).GetString().Trim(),
                Gender = worksheet.Cell(row, 7).GetString().Trim(),
                DateOfBirth = ParseDate(worksheet.Cell(row, 8).GetString()) ?? DateTime.MinValue,
                Nationality = worksheet.Cell(row, 9).GetString().Trim(),
                JobService = worksheet.Cell(row, 10).GetString().Trim(),
                Education = worksheet.Cell(row, 11).GetString().Trim(),
                Country = worksheet.Cell(row, 12).GetString().Trim(),
                Address = worksheet.Cell(row, 13).GetString().Trim(),
                HiringDate = ParseDate(worksheet.Cell(row, 14).GetString()) ?? DateTime.MinValue,
                HiringType = worksheet.Cell(row, 15).GetString().Trim(),
                WorkplaceAddress = worksheet.Cell(row, 16).GetString().Trim(),
                EmployeeNumber = worksheet.Cell(row, 17).GetString().Trim(),
                MonthlySalary = TryParseDecimal(worksheet.Cell(row, 18).GetString()),
                Allowances = TryParseDecimal(worksheet.Cell(row, 19).GetString()),
            };
        }
        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentNullException(nameof(file), "File is invalid");

            if (!IsExcelFile(file.FileName))
            {
                var errors = new List<string> { "The file is not a valid Excel file." };
                throw new ExcelException("Excel file validation failed.", errors);
            }
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

        private bool IsExcelFile(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            return extension == "x.ls" || extension == ".xlsx";
        }

    }
}

