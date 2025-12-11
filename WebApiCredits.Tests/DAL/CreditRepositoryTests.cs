using Microsoft.Data.Sqlite;
using WebApiCredits.DAL;

namespace WebApiCredits.Tests.DAL
{
    public class CreditRepositoryTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly CreditRepository _repository;

        public CreditRepositoryTests()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            DatabaseInitializer initializer = new(_connection);
            initializer.Initialize();

            _repository = new CreditRepository(_connection);
        }

        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }

        [Fact]
        public async Task GetAllCreditsWithInvoicesAsync_ReturnsData()
        {
            var result = await _repository.GetAllCreditsWithInvoicesAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetCreditSummaryAsync_ReturnsCorrectSummary()
        {
            var result = await _repository.GetCreditSummaryAsync();

            Assert.NotNull(result);
            Assert.True(result.TotalPaidAmount >= 0);
            Assert.True(result.TotalAwaitingPaymentAmount >= 0);

            Assert.InRange(result.PaidPercentage, 0, 100);
            Assert.InRange(result.AwaitingPaymentPercentage, 0, 100);
        }

        [Fact]
        public async Task GetAllCreditsWithInvoicesAsync_ReturnsCreditsWithInvoices()
        {
            var result = await _repository.GetAllCreditsWithInvoicesAsync();
            var credit = result.FirstOrDefault();

            Assert.NotNull(credit);
            Assert.NotNull(credit.Invoices); 
        }
    }
}