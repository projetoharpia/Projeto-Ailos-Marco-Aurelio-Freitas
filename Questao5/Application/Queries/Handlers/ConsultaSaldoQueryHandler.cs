using MediatR;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Application.Results;
using Questao5.Infrastructure.Database.QueryStore;

namespace Questao5.Application.Queries.Handlers;

public class ConsultaSaldoQueryHandler : IRequestHandler<ConsultaSaldoQuery, BaseResult<ContaCorrenteSaldoDto>>
{
    private readonly IConsultaSaldoQueryStore _queryStore;

    public ConsultaSaldoQueryHandler(IConsultaSaldoQueryStore queryStore)
    {
        _queryStore = queryStore;
    }

    public async Task<BaseResult<ContaCorrenteSaldoDto>> Handle(ConsultaSaldoQuery request, CancellationToken cancellationToken)
    {
        var conta = await _queryStore.GetContaCorrenteByNumeroAsync(request.NumeroConta);

        if (conta == null)
            return BaseResult<ContaCorrenteSaldoDto>.Failure("Conta não cadastrada", "INVALID_ACCOUNT");

        if (!conta.Ativo)
            return BaseResult<ContaCorrenteSaldoDto>.Failure("Conta inativa", "INACTIVE_ACCOUNT");

        var saldo = await _queryStore.GetSaldoByIdContaCorrenteAsync(conta.IdContaCorrente!);

        var resultDto = new ContaCorrenteSaldoDto
        {
            NumeroConta = conta.Numero,
            NomeTitular = conta.Nome,
            SaldoAtual = saldo
        };

        return BaseResult<ContaCorrenteSaldoDto>.Success(resultDto);
    }
}