using EB.Services;
using Microsoft.AspNetCore.Mvc;

namespace EB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IBankHoursService _service;

        public ImportController(IBankHoursService service)
        {
            _service = service;
        }

        [HttpPost("disk")]
        public async Task<IActionResult> ImportFromDisk()
        {
            // �������� �� path ���� �� ������
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "bank_hours.xlsx");

            try
            {
                await _service.ImportFromExcelFromDiskAsync(filePath);
                return Ok("� �������� ��� ������ ������������!");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound($"��� ������� �� ������ Excel: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"������ ���� ��� ��������: {ex.Message}");
            }
        }

    }

}
