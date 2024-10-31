using NSubstitute;
using Questao5.Application.Commands.Handlers;
using Questao5.Application.Commands.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.CommandStore;
using Xunit;

namespace Questao5.Tests.Handlers;

public class MovimentacaoCommandHandlerTests
{
    private readonly IMovimentacaoCommandStore _commandStore;
    private readonly MovimentacaoCommandHandler _handler;

    public MovimentacaoCommandHandlerTests()
    {
        _commandStore = Substitute.For<IMovimentacaoCommandStore>();
        _handler = new MovimentacaoCommandHandler(_commandStore);
    }

    [Fact]
    public async Task Handle_ContaInativa_ReturnsFailure()
    {
        // Arrange
        var command = new MovimentacaoCommand
        {
            IdContaCorrente = "id",
            Valor = 100,
            TipoMovimento = 'C',
            ChaveIdempotencia = "key"
        };

        _commandStore.GetContaCorrenteByIdAsync(command.IdContaCorrente).Returns(new ContaCorrente { Ativo = false });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Conta inativa", result.Message);
    }

    [Fact]
    public async Task Handle_ContaNaoCadastrada_ReturnsFailure()
    {
        // Arrange
        var command = new MovimentacaoCommand
        {
            IdContaCorrente = "id",
            Valor = 100,
            TipoMovimento = 'C',
            ChaveIdempotencia = "key"
        };

        _commandStore.GetContaCorrenteByIdAsync(command.IdContaCorrente).Returns((ContaCorrente)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Conta não cadastrada", result.Message);
    }

    [Fact]
    public async Task Handle_ValorInvalido_ReturnsFailure()
    {
        // Arrange
        var command = new MovimentacaoCommand
        {
            IdContaCorrente = "id",
            Valor = -100,
            TipoMovimento = 'C',
            ChaveIdempotencia = "key"
        };

        _commandStore.GetContaCorrenteByIdAsync(command.IdContaCorrente).Returns(new ContaCorrente { Ativo = true });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Valor inválido", result.Message);
    }

    [Fact]
    public async Task Handle_IdentificacaoIncorreta_ReturnsFailure()
    {
        // Arrange
        var command = new MovimentacaoCommand
        {
            IdContaCorrente = "id",
            Valor = 100,
            TipoMovimento = 'C',
            ChaveIdempotencia = "key"
        };

        _commandStore.GetContaCorrenteByIdAsync(command.IdContaCorrente).Returns(new ContaCorrente { Ativo = true });
        _commandStore.GetIdempotenciaByChaveAsync(command.ChaveIdempotencia).Returns(new Idempotencia());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Identificação Incorreta", result.Message);
    }

    [Fact]
    public async Task Handle_TipoMovimentoInvalido_ReturnsFailure()
    {
        // Arrange
        var command = new MovimentacaoCommand
        {
            IdContaCorrente = "id",
            Valor = 100,
            TipoMovimento = 'X',
            ChaveIdempotencia = "key"
        };

        _commandStore.GetContaCorrenteByIdAsync(command.IdContaCorrente).Returns(new ContaCorrente { Ativo = true });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Tipo inválido", result.Message);
    }


    [Fact]
    public async Task Handle_Valido_ReturnsSuccess()
    {
        // Arrange
        var command = new MovimentacaoCommand
        {
            IdContaCorrente = "id",
            Valor = 100,
            TipoMovimento = 'C',
            ChaveIdempotencia = "key"
        };

        _commandStore.GetContaCorrenteByIdAsync(command.IdContaCorrente).Returns(new ContaCorrente { Ativo = true });
        _commandStore.GetIdempotenciaByChaveAsync(command.ChaveIdempotencia).Returns((Idempotencia)null!);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.False(string.IsNullOrEmpty(result.Data));
    }
}