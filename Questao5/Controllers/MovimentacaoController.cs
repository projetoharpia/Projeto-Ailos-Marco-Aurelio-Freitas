using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Requests;

namespace Questao5.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimentacaoController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovimentacaoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Realiza uma movimentação na conta corrente (Crédito ou Débito).
    /// </summary>
    /// <param name="command">Dados da movimentação</param>
    /// <returns>Resultado da operação</returns>
    [HttpPost("movimentar")]
    public async Task<IActionResult> MovimentarConta([FromBody] MovimentacaoCommand command)
    {
        var result = await _mediator.Send(command);

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Message, code = result.ErrorCode });
        }

        return Ok(new { idMovimento = result.Data });
    }

    /// <summary>
    /// Consulta o saldo da conta corrente.
    /// </summary>
    /// <param name="numeroConta">Número da conta a ser consultada</param>
    /// <returns>Saldo atual da conta corrente</returns>
    [HttpGet("saldo/{numeroConta}")]
    public async Task<IActionResult> ConsultarSaldo(int numeroConta)
    {
        var query = new ConsultaSaldoQuery { NumeroConta = numeroConta };
        var result = await _mediator.Send(query);

        if (!result.IsSuccess)
        {
            return BadRequest(new { message = result.Message, code = result.ErrorCode });
        }

        return Ok(new
        {
            numeroConta,
            titular = result.Data!.NomeTitular,
            dataConsulta = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
            saldo = result.Data.SaldoAtual
        });
    }
}
