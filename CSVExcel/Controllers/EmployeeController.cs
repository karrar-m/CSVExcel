using CSVExcel.Exceptions;
using CSVExcel.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace CSVExcel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeService employeeService) : ControllerBase
    {
        private readonly IEmployeeService _employeeService = employeeService;

        [HttpPost("upload")]
        public async Task<ActionResult> UploadFile(IFormFile file, CancellationToken cancellationToken)
        {
            try
            {
              bool isupload =   await _employeeService.ExcelEmployee(file  ,cancellationToken);
                return Ok(new {  isupload ,  Message = "تم رفع الموظفين بنجاح" });
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