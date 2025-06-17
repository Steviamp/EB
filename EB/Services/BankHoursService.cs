using EB.DbContextData;
using EB.Models;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace EB.Services
{
    public class BankHoursService : IBankHoursService
    {
        private readonly AppDbContext _context;

        public BankHoursService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ImportFromExcelAsync(IFormFile file)
        {
            using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            using var package = new ExcelPackage(stream);
            var worksheet = package.Workbook.Worksheets[0];

            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
            {
                var code = worksheet.Cells[row, 1].Text.Trim();
                var from = TimeSpan.Parse(worksheet.Cells[row, 2].Text);
                var to = TimeSpan.Parse(worksheet.Cells[row, 3].Text);

                var existing = await _context.BankHours.FirstOrDefaultAsync(b => b.Code == code);
                if (existing != null)
                {
                    existing.From = from;
                    existing.To = to;
                }
                else
                {
                    _context.BankHours.Add(new BankHours { Code = code, From = from, To = to });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<BankHours?> GetBankHoursAsync(string code)
        {
            return await _context.BankHours.FirstOrDefaultAsync(b => b.Code == code);
        }
    }

}
