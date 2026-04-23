using FluentAssertions;
using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Servicos;
using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Excecoes;
using GoodHamburger.Domain.Interfaces;
using NSubstitute;

namespace GoodHamburger.Tests.Aplicacao;

public class PedidoServicoTestes
{
    private readonly IPedidoRepositorio _pedidoRepo = Substitute.For<IPedidoRepositorio>();
    private readonly ICardapioRepositorio _cardapioRepo = Substitute.For<ICardapioRepositorio>();
    private readonly PedidoServico _servico;

    private static readonly ItemCardapio XBurger     = new("X-BURGER", "X Burger",     5.00m, TipoItem.Sanduiche);
    private static readonly ItemCardapio Batata       = new("BATATA",   "Batata Frita", 2.00m, TipoItem.Acompanhamento);
    private static readonly ItemCardapio Refrigerante = new("REFRI",    "Refrigerante", 2.50m, TipoItem.Bebida);

    public PedidoServicoTestes()
    {
        _cardapioRepo.ObterPorCodigo("X-BURGER").Returns(XBurger);
        _cardapioRepo.ObterPorCodigo("BATATA").Returns(Batata);
        _cardapioRepo.ObterPorCodigo("REFRI").Returns(Refrigerante);
        _servico = new PedidoServico(_pedidoRepo, _cardapioRepo);
    }

    [Fact]
    public async Task Criar_ComItensValidos_DeveRetornarPedidoComDesconto()
    {
        var dto = new CriarPedidoDto(["X-BURGER", "BATATA", "REFRI"]);

        var resultado = await _servico.Criar(dto);

        resultado.PercentualDesconto.Should().Be(0.20m);
        resultado.Total.Should().Be(7.60m);
        await _pedidoRepo.Received(1).Adicionar(Arg.Any<Pedido>());
    }

    [Fact]
    public async Task Criar_ComCodigoInexistente_DeveLancarDomainException()
    {
        _cardapioRepo.ObterPorCodigo("INVALIDO").Returns((ItemCardapio?)null);
        var dto = new CriarPedidoDto(["INVALIDO"]);

        var acao = async () => await _servico.Criar(dto);

        await acao.Should().ThrowAsync<DomainException>()
            .WithMessage("*não encontrado no cardápio*");
    }

    [Fact]
    public async Task ObterPorId_ComIdInexistente_DeveLancarRecursoNaoEncontrado()
    {
        var id = Guid.NewGuid();
        _pedidoRepo.ObterPorId(id).Returns((Pedido?)null);

        var acao = async () => await _servico.ObterPorId(id);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task Remover_ComIdInexistente_DeveLancarRecursoNaoEncontrado()
    {
        var id = Guid.NewGuid();
        _pedidoRepo.ObterPorId(id).Returns((Pedido?)null);

        var acao = async () => await _servico.Remover(id);

        await acao.Should().ThrowAsync<RecursoNaoEncontradoException>();
    }

    [Fact]
    public async Task Listar_DeveRetornarTodosOsPedidos()
    {
        var pedido = new Pedido([XBurger]);
        _pedidoRepo.Listar().Returns(new List<Pedido> { pedido });

        var resultado = await _servico.Listar();

        resultado.Should().HaveCount(1);
    }
}
