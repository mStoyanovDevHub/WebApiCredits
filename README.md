Credits API – Task Description

This project implements a REST API for managing credit information using .NET 8, Dapper, and an in-memory SQLite database.

The task requirements are:

1. Build a data model consisting of Credits and Invoices.
   - Each credit has:
       • Credit number
       • Client name
       • Requested amount
       • Date of creation/request
       • Status (Paid, AwaitingPayment, Created)
   - Each credit may have an unlimited number of invoices.
   - Each invoice has:
       • Invoice number
       • Invoice amount

2. Database structure and relations
   - Table: Credits
       • Id (PK)
       • CreditNumber
       • ClientName
       • RequestedAmount
       • CreatedOn
       • Status
   - Table: Invoices
       • Id (PK)
       • InvoiceNumber
       • Amount
       • CreditId (FK → Credits.Id)

   Relation:
       Credits 1 → Many Invoices
       (Each credit can have multiple invoices; every invoice belongs to exactly one credit.)

3. Use an in-memory SQLite database.
   - The schema is created on application startup.
   - Test seed data is inserted on every run.

4. Implement two API endpoints:
   • Endpoint returning all credits together with their invoices.
   • Endpoint returning:
        - total sum of credits with status Paid
        - total sum of credits with status AwaitingPayment
        - percentage of each amount relative to the combined total of Paid + AwaitingPayment

5. Use Dapper for all database operations.

6. Tests:
   • DatabaseInitializerTests
       - Confirms that both tables (Credits, Invoices) are created correctly.
       - Ensures seed data is inserted on each initialization.

   • CreditRepositoryTests
       - Verifies that credits with invoices can be retrieved from the database.
       - Checks that summary calculations (totals + percentages) return valid values.
       - Ensures each returned Credit includes its related Invoice collection.

   • CreditControllerTests
       - Validates controller responses using mocked repositories.
       - Ensures the /credits endpoint returns a list of credits with invoices.
       - Ensures the /credits/summary endpoint returns the expected summary object.
