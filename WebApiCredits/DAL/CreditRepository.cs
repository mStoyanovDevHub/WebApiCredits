using Dapper;
using Microsoft.Data.Sqlite;
using WebApiCredits.Models;
using WebApiCredits.Models.Enums;

namespace WebApiCredits.DAL
{
    public class CreditRepository(SqliteConnection connection) : ICreditRepository
    {
        private readonly SqliteConnection _connection = connection;

        public async Task<IEnumerable<Credit>> GetAllCreditsWithInvoicesAsync()
        {
            const string sql = @"
            SELECT
                c.Id,
                c.CreditNumber,
                c.CustomerName,
                c.RequestedAmount,
                c.RequestDate,
                c.Status,
                i.Id AS InvoiceId,
                i.InvoiceNumber,
                i.Amount,
                i.CreditId
            FROM Credits c
            LEFT JOIN Invoices i ON c.Id = i.CreditId
            ORDER BY c.Id, i.Id";

            var dict = new Dictionary<int, Credit>();

            await _connection.QueryAsync<Credit, Invoice, Credit>(
                sql,
                (credit, invoice) =>
                {
                    if (!dict.TryGetValue(credit.Id, out var entry))
                    {
                        entry = credit;
                        entry.Invoices = new List<Invoice>();
                        dict.Add(entry.Id, entry);
                    }

                    if (invoice is not null && invoice.InvoiceId > 0)
                    {
                        invoice.Id = invoice.InvoiceId; // copy alias into Id
                        entry.Invoices.Add(invoice);
                    }

                    return entry;
                },
                splitOn: "InvoiceId"
            );

            return dict.Values.OrderBy(c => c.Id).ToList();
        }

        public async Task<CreditSummary> GetCreditSummaryAsync()
        {
            var paid = (int)CreditStatus.Paid;
            var awaiting = (int)CreditStatus.AwaitingPayment;

            const string sql = @"
            SELECT 
                SUM(CASE WHEN Status = @paid THEN RequestedAmount ELSE 0 END) as TotalPaidAmount,
                SUM(CASE WHEN Status = @awaiting THEN RequestedAmount ELSE 0 END) as TotalAwaitingPaymentAmount,
                SUM(RequestedAmount) as TotalAmount
            FROM Credits 
            WHERE Status IN (@awaiting, @paid)";

            var result = await _connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { paid, awaiting });

            if (result == null || result.TotalAmount == null || result.TotalAmount == 0)
                return new CreditSummary();

            decimal totalPaid = result.TotalPaidAmount ?? 0;
            decimal totalAwaiting = result.TotalAwaitingPaymentAmount ?? 0;
            decimal total = result.TotalAmount ?? 1;

            return new CreditSummary
            {
                TotalPaidAmount = totalPaid,
                TotalAwaitingPaymentAmount = totalAwaiting,
                PaidPercentage = totalPaid / total * 100,
                AwaitingPaymentPercentage = totalAwaiting / total * 100
            };
        }
    }
}