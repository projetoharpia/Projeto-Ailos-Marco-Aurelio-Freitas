using Dapper;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database.CommandStore;

public class MovimentacaoCommandStore : IMovimentacaoCommandStore
{
    private readonly IDbConnection _dbConnection;

    public MovimentacaoCommandStore(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ContaCorrente> GetContaCorrenteByIdAsync(string idContaCorrente)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<ContaCorrente>(
            "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente",
            new { IdContaCorrente = idContaCorrente });
    }

    public async Task<Idempotencia> GetIdempotenciaByChaveAsync(string chaveIdempotencia)
    {
        return await _dbConnection.QueryFirstOrDefaultAsync<Idempotencia>(
            "SELECT * FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia",
            new { ChaveIdempotencia = chaveIdempotencia });
    }

    public async Task InsertMovimentoAsync(string idMovimento, string idContaCorrente, string dataMovimento, char tipoMovimento, decimal valor)
    {
        await _dbConnection.ExecuteAsync(
            "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)",
            new { IdMovimento = idMovimento, IdContaCorrente = idContaCorrente, DataMovimento = dataMovimento, TipoMovimento = tipoMovimento, Valor = valor });
    }

    public async Task InsertIdempotenciaAsync(string chaveIdempotencia, string requisicao, string resultado)
    {
        await _dbConnection.ExecuteAsync(
            "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)",
            new { ChaveIdempotencia = chaveIdempotencia, Requisicao = requisicao, Resultado = resultado });
    }
}