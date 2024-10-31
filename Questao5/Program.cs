using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.CommandStore;
using Questao5.Infrastructure.Database.QueryStore;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite");
builder.Services.AddSingleton(new DatabaseConfig { Name = connectionString });

builder.Services.AddScoped<IDbConnection>(db => new SqliteConnection(connectionString));

builder.Services.AddControllers();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<IMovimentacaoCommandStore, MovimentacaoCommandStore>();
builder.Services.AddTransient<IConsultaSaldoQueryStore, ConsultaSaldoQueryStore>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Services.GetService<IDatabaseBootstrap>()?.Setup();

app.Run();