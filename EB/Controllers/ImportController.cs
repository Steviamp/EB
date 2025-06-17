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

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty");

            await _service.ImportFromExcelAsync(file);
            return Ok("Imported successfully");
        }
    }

}
