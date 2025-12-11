using Microsoft.Data.Sqlite;
using WebApiCredits.DAL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<SqliteConnection>(sp =>
{
    SqliteConnection connection = new("Data Source=:memory:");
    connection.Open();

    DatabaseInitializer initializer = new(connection);
    initializer.Initialize();

    return connection;
});

builder.Services.AddScoped<ICreditRepository, CreditRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();