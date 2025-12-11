using System.Collections.Generic;
using WebApiCredits.Models;

namespace WebApiCredits.DAL
{
    public interface ICreditRepository
    {
        Task<IEnumerable<Credit>> GetAllCreditsWithInvoicesAsync();
        Task<CreditSummary> GetCreditSummaryAsync();
    }
}