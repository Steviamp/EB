using EB.Models;

namespace EB.Services
{
    public interface IBankHoursService
    {
        Task ImportFromExcelFromDiskAsync(string filePath);
        Task<BankHours?> GetBankHoursAsync(string code);
    }

}
