using Dapper;
using Microsoft.Data.Sqlite;
using WebApiCredits.DAL;

namespace WebApiCredits.Tests.Integration
{
    public class DatabaseInitializerTests
    {
        [Fact]
        public void Initialize_CreatesTables()
        {
            SqliteConnection connection = new("Data Source=:memory:");
            connection.Open();
            DatabaseInitializer initializer = new(connection);

            initializer.Initialize();

            var creditsTable = connection.Query<string>(
                "SELECT name FROM sqlite_master WHERE type='table' AND name='Credits'");
            Assert.Single(creditsTable);

            var invoicesTable = connection.Query<string>(
                "SELECT name FROM sqlite_master WHERE type='table' AND name='Invoices'");
            Assert.Single(invoicesTable);
        }

        [Fact]
        public void Initialize_SeedsData()
        {
            SqliteConnection connection = new("Data Source=:memory:");
            connection.Open();
            DatabaseInitializer initializer = new(connection);

            initializer.Initialize();

            var creditCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Credits");
            Assert.True(creditCount > 0);

            var invoiceCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Invoices");
            Assert.True(invoiceCount > 0);
        }
    }
}