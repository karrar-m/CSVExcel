using CSVExcel.Exceptions;
using CSVExcel.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CSVExcel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadEmployeeFile(IFormFile file)
        {
            try
            {
                await _employeeService.ExcelEmployee(file);
                return Ok(new { Message = "تم رفع الموظفين بنجاح" });
            }
            catch (ExcelException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    Errors = ex.Errors
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "حدث خطأ غير متوقع" });
            }
        }

    }
}