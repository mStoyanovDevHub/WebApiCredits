using System.Text.Json.Serialization;

namespace WebApiCredits.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        [JsonIgnore]
        public int InvoiceId { get; set; } // for Dapper alias
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int CreditId { get; set; }
    }
}