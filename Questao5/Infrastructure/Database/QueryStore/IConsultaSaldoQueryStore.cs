using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.QueryStore;

public interface IConsultaSaldoQueryStore
{
    Task<ContaCorrente> GetContaCorrenteByNumeroAsync(int numeroConta);
    Task<decimal> GetSaldoByIdContaCorrenteAsync(string idContaCorrente);
}