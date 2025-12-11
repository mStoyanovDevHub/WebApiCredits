namespace WebApiCredits.Models
{
    public class CreditSummary
    {
        public decimal TotalPaidAmount { get; set; }
        public decimal TotalAwaitingPaymentAmount { get; set; }
        public decimal PaidPercentage { get; set; }
        public decimal AwaitingPaymentPercentage { get; set; }
    }
}