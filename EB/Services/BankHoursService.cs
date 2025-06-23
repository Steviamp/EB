using EB.DbContextData;
using EB.Models;
using Microsoft.EntityFrameworkCore;
using ClosedXML.Excel;

namespace EB.Services
{
    public class BankHoursService : IBankHoursService
    {
        private readonly AppDbContext _context;

        public BankHoursService(AppDbContext context)
        {
            _context = context;
        }

        public async Task ImportFromExcelFromDiskAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Excel αρχείο δεν βρέθηκε", filePath);

            using var workbook = new XLWorkbook(filePath);
            var worksheet = workbook.Worksheet(1);

            var rows = worksheet.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                var code = row.Cell(1).GetString().Trim();
                var from = TimeSpan.Parse(row.Cell(2).GetString());
                var to = TimeSpan.Parse(row.Cell(3).GetString());

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
