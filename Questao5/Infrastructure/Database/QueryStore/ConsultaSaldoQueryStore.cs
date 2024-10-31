using Dapper;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database.QueryStore;

public class ConsultaSaldoQueryStore : IConsultaSaldoQueryStore
{
    private readonly IDbConnection _dbConnection;

    public ConsultaSaldoQueryStore(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ContaCorrente> GetContaCorrenteByNumeroAsync(int numeroConta)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
            "SELECT * FROM contacorrente WHERE numero = @NumeroConta",
            new { NumeroConta = numeroConta });
    }

    public async Task<decimal> GetSaldoByIdContaCorrenteAsync(string idContaCorrente)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<decimal>(
            @"SELECT 
                    IFNULL(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END), 0) - 
                    IFNULL(SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END), 0) AS Saldo 
                  FROM movimento 
                  WHERE idcontacorrente = @IdContaCorrente",
            new { IdContaCorrente = idContaCorrente });
    }
}