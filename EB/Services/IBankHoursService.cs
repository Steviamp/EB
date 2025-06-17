using EB.Models;

namespace EB.Services
{
    public interface IBankHoursService
    {
        Task ImportFromExcelAsync(IFormFile file);
        Task<BankHours?> GetBankHoursAsync(string code);
    }

}
