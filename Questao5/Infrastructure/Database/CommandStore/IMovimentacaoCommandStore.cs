using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.CommandStore;

public interface IMovimentacaoCommandStore
{
    Task<ContaCorrente> GetContaCorrenteByIdAsync(string idContaCorrente);
    Task<Idempotencia> GetIdempotenciaByChaveAsync(string chaveIdempotencia);
    Task InsertMovimentoAsync(string idMovimento, string idContaCorrente, string dataMovimento, char tipoMovimento, decimal valor);
    Task InsertIdempotenciaAsync(string chaveIdempotencia, string requisicao, string resultado);
}
