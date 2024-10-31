using MediatR;
using Questao5.Application.Results;

namespace Questao5.Application.Commands.Requests;

public class MovimentacaoCommand : IRequest<BaseResult<string>>
{
    public string? ChaveIdempotencia { get; set; }
    public string? IdContaCorrente { get; set; }
    public decimal Valor { get; set; }
    public char TipoMovimento { get; set; }
}