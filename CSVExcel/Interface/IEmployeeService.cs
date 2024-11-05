using CSVExcel.Model.Employee;

namespace CSVExcel.Interface;

public interface IEmployeeService
{
    Task<bool> ExcelEmployee (IFormFile file , CancellationToken cancellationToken);
}