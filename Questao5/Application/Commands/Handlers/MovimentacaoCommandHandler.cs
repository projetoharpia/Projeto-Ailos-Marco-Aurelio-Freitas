using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Messages;
using Questao5.Application.Results;
using Questao5.Infrastructure.Database.CommandStore;

namespace Questao5.Application.Commands.Handlers;

public class MovimentacaoCommandHandler : IRequestHandler<MovimentacaoCommand, BaseResult<string>>
{
    private readonly IMovimentacaoCommandStore _commandStore;

    public MovimentacaoCommandHandler(IMovimentacaoCommandStore commandStore)
    {
        _commandStore = commandStore;
    }

    public async Task<BaseResult<string>> Handle(MovimentacaoCommand request, CancellationToken cancellationToken)
    {
        var conta = await _commandStore.GetContaCorrenteByIdAsync(request.IdContaCorrente!);

        if (conta == null) return BaseResult<string>.Failure(GenericMessages.ContaNaoCadastrada, "INVALID_ACCOUNT");
        if (!conta.Ativo) return BaseResult<string>.Failure(GenericMessages.ContaInativa, "INACTIVE_ACCOUNT");
        if (request.Valor <= 0) return BaseResult<string>.Failure(GenericMessages.ValorInvalido, "INVALID_VALUE");
        char tipoMovimento = request.TipoMovimento;
        if (tipoMovimento != 'C' && tipoMovimento != 'D')
        {
            return BaseResult<string>.Failure(GenericMessages.TipoInvalido, "INVALID_TYPE");
        }

        var idempotencia = await _commandStore.GetIdempotenciaByChaveAsync(request.ChaveIdempotencia!);
        if (idempotencia != null) return BaseResult<string>.Failure(GenericMessages.IdentificacaoIncorreta, "INVALID_ID");

        var idMovimento = Guid.NewGuid().ToString();
        var dataMovimento = DateTime.Now.ToString("dd/MM/yyyy");
        await _commandStore.InsertMovimentoAsync(idMovimento, request.IdContaCorrente!, dataMovimento, tipoMovimento, request.Valor);

        var resultado = BaseResult<string>.Success(idMovimento);
        await _commandStore.InsertIdempotenciaAsync(request.ChaveIdempotencia!, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(resultado));

        return resultado;
    }
}