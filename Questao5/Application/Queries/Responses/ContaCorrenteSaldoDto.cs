namespace Questao5.Application.Queries.Responses;

public class ContaCorrenteSaldoDto
{
    public int? NumeroConta { get; set; }
    public string? NomeTitular { get; set; }
    public DateTime DataConsulta { get; set; }
    public decimal SaldoAtual { get; set; }
}