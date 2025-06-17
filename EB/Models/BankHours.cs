namespace EB.Models
{
    public class BankHours
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }

}
