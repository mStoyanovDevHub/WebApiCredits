using WebApiCredits.Models.Enums;

namespace WebApiCredits.Models
{
    public class Credit
    {
        public int Id { get; set; }
        public string CreditNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public decimal RequestedAmount { get; set; }
        public DateTime RequestDate { get; set; }
        public CreditStatus Status { get; set; }
        public List<Invoice> Invoices { get; set; } = [];
    }
}