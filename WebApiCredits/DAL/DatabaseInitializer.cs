using Dapper;
using Microsoft.Data.Sqlite;

namespace WebApiCredits.DAL
{
    public class DatabaseInitializer(SqliteConnection connection)
    {
        private readonly SqliteConnection _connection = connection;

        public void Initialize()
        {
            CreateTables();
            SeedData();
        }

        private void CreateTables()
        {
            _connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Credits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    CreditNumber TEXT NOT NULL,
                    CustomerName TEXT NOT NULL,
                    RequestedAmount DECIMAL(18,2) NOT NULL,
                    RequestDate DATETIME NOT NULL,
                    Status INTEGER NOT NULL CHECK (Status IN (0, 1, 2))
                )");

            _connection.Execute(@"
                CREATE TABLE IF NOT EXISTS Invoices (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    InvoiceNumber TEXT NOT NULL,
                    Amount DECIMAL(18,2) NOT NULL,
                    CreditId INTEGER NOT NULL,
                    FOREIGN KEY (CreditId) REFERENCES Credits(Id)
                )");
        }

        private void SeedData()
        {
            var creditCount = _connection.ExecuteScalar<int>("SELECT COUNT(*) FROM Credits");

            if (creditCount == 0)
            {
                _connection.Execute(@"
                    INSERT INTO Credits (CreditNumber, CustomerName, RequestedAmount, RequestDate, Status)
                    VALUES 
                    ('CRD001', 'Sean James', 5000.00, '2024-01-15', 2),
                    ('CRD002', 'John Doe', 10000.00, '2024-02-20', 1),
                    ('CRD003', 'Dan Clark', 7500.00, '2024-03-10', 2),
                    ('CRD004', 'Ana Belick', 3000.00, '2024-03-25', 0),
                    ('CRD005', 'Pete Basil', 12000.00, '2024-04-05', 1)");

                _connection.Execute(@"
                    INSERT INTO Invoices (InvoiceNumber, Amount, CreditId)
                    VALUES 
                    ('INV001', 1500.00, 1),
                    ('INV002', 2000.00, 1),
                    ('INV003', 1000.00, 2),
                    ('INV004', 5000.00, 3),
                    ('INV005', 2500.00, 3),
                    ('INV006', 3000.00, 5)");
            }
        }
    }
}