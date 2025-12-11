using Dapper;
using Microsoft.Data.Sqlite;
using WebApiCredits.Models;

namespace WebApiCredits.DAL
{
    public class CreditRepository(SqliteConnection connection) : ICreditRepository
    {
        private readonly SqliteConnection _connection = connection;

        public async Task<IEnumerable<Credit>> GetAllCreditsWithInvoicesAsync()
        {
            var sql = @"
                SELECT c.*, i.Id, i.InvoiceNumber, i.Amount, i.CreditId 
                FROM Credits c
                LEFT JOIN Invoices i ON c.Id = i.CreditId
                ORDER BY c.Id, i.Id";

            Dictionary<int, Credit> creditDictionary = [];

            await _connection.QueryAsync<Credit, Invoice, Credit>(
                sql,
                (credit, invoice) =>
                {
                    if (!creditDictionary.TryGetValue(credit.Id, out var creditEntry))
                    {
                        creditEntry = credit;
                        creditEntry.Invoices = new List<Invoice>();
                        creditDictionary.Add(creditEntry.Id, creditEntry);
                    }

                    if (invoice != null && invoice.Id > 0)
                    {
                        creditEntry.Invoices.Add(invoice);
                    }

                    return creditEntry;
                },
                splitOn: "Id"  
            );

            return creditDictionary.Values;
        }

        public async Task<CreditSummary> GetCreditSummaryAsync()
        {
            var sql = @"
                SELECT 
                    SUM(CASE WHEN Status = 2 THEN RequestedAmount ELSE 0 END) as TotalPaidAmount,
                    SUM(CASE WHEN Status = 1 THEN RequestedAmount ELSE 0 END) as TotalAwaitingPaymentAmount,
                    SUM(RequestedAmount) as TotalAmount
                FROM Credits 
                WHERE Status IN (1, 2)";

            var result = await _connection.QueryFirstOrDefaultAsync<dynamic>(sql);

            if (result == null || result.TotalAmount == null || result.TotalAmount == 0)
            {
                return new CreditSummary();
            }

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