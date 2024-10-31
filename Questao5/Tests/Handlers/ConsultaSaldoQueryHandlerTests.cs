using NSubstitute;
using Questao5.Application.Queries.Handlers;
using Questao5.Application.Queries.Requests;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.QueryStore;
using Xunit;

namespace Questao5.Tests.Handlers
{
    public class ConsultaSaldoQueryHandlerTests
    {
        private readonly IConsultaSaldoQueryStore _queryStore;
        private readonly ConsultaSaldoQueryHandler _handler;

        public ConsultaSaldoQueryHandlerTests()
        {
            _queryStore = Substitute.For<IConsultaSaldoQueryStore>();
            _handler = new ConsultaSaldoQueryHandler(_queryStore);
        }

        [Fact]
        public async Task Handle_ContaNaoCadastrada_ReturnsFailure()
        {
            // Arrange
            var request = new ConsultaSaldoQuery
            {
                NumeroConta = 999
            };

            _queryStore.GetContaCorrenteByNumeroAsync(request.NumeroConta)!.Returns((ContaCorrente)null!);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Conta não cadastrada", result.Message);
        }

        [Fact]
        public async Task Handle_ContaInativa_ReturnsFailure()
        {
            // Arrange
            var request = new ConsultaSaldoQuery
            {
                NumeroConta = 999
            };

            var conta = new ContaCorrente
            {
                Ativo = false,
                Numero = request.NumeroConta,
                Nome = "Titular Inativo"
            };

            _queryStore.GetContaCorrenteByNumeroAsync(request.NumeroConta).Returns(conta);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Conta inativa", result.Message);
        }

        [Fact]
        public async Task Handle_ContaAtiva_ReturnsSuccess()
        {
            // Arrange
            var request = new ConsultaSaldoQuery
            {
                NumeroConta = 123
            };

            var conta = new ContaCorrente
            {
                Ativo = true,
                IdContaCorrente = "123",
                Numero = request.NumeroConta,
                Nome = "Titular Ativo"
            };

            var saldoEsperado = 1000m;

            _queryStore.GetContaCorrenteByNumeroAsync(request.NumeroConta).Returns(conta);
            _queryStore.GetSaldoByIdContaCorrenteAsync(conta.IdContaCorrente).Returns(saldoEsperado);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(conta.Numero, result.Data!.NumeroConta);
            Assert.Equal(conta.Nome, result.Data.NomeTitular);
            Assert.Equal(saldoEsperado, result.Data.SaldoAtual);
        }
    }
}