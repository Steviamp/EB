using EB.Services;
using Microsoft.AspNetCore.Mvc;

namespace EB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankController : ControllerBase
    {
        private readonly IBankHoursService _service;

        public BankController(IBankHoursService service)
        {
            _service = service;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetHours(string code)
        {
            var bank = await _service.GetBankHoursAsync(code);
            if (bank == null)
                return NotFound("Bank code not found");

            return Ok(new
            {
                bank.Code,
                From = bank.From.ToString(@"hh\:mm"),
                To = bank.To.ToString(@"hh\:mm")
            });
        }
    }

}
