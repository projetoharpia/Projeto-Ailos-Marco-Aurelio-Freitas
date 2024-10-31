using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Application.Results;

namespace Questao5.Application.Queries.Requests;

public class ConsultaSaldoQuery : IRequest<BaseResult<ContaCorrenteSaldoDto>>
{
    public int NumeroConta { get; set; }
}