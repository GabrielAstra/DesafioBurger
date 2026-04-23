using FluentAssertions;
using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Excecoes;

namespace GoodHamburger.Tests.Dominio;

public class PedidoTestes
{
    private static readonly ItemCardapio XBurger    = new("X-BURGER", "X Burger",     5.00m, TipoItem.Sanduiche);
    private static readonly ItemCardapio XEgg       = new("X-EGG",    "X Egg",        4.50m, TipoItem.Sanduiche);
    private static readonly ItemCardapio Batata      = new("BATATA",   "Batata Frita", 2.00m, TipoItem.Acompanhamento);
    private static readonly ItemCardapio Refrigerante = new("REFRI",  "Refrigerante", 2.50m, TipoItem.Bebida);

    [Fact]
    public void Pedido_ComSanduicheBatataRefri_DeveAplicar20PorCentoDeDesconto()
    {
        var pedido = new Pedido([XBurger, Batata, Refrigerante]);

        pedido.PercentualDesconto.Should().Be(0.20m);
        pedido.Subtotal.Should().Be(9.50m);
        pedido.Desconto.Should().Be(1.90m);
        pedido.Total.Should().Be(7.60m);
    }

    [Fact]
    public void Pedido_ComSanduicheERefri_DeveAplicar15PorCentoDeDesconto()
    {
        var pedido = new Pedido([XBurger, Refrigerante]);

        pedido.PercentualDesconto.Should().Be(0.15m);
        pedido.Subtotal.Should().Be(7.50m);
        pedido.Desconto.Should().Be(1.12m);
        pedido.Total.Should().Be(6.38m);
    }

    [Fact]
    public void Pedido_ComSanduicheEBatata_DeveAplicar10PorCentoDeDesconto()
    {
        var pedido = new Pedido([XBurger, Batata]);

        pedido.PercentualDesconto.Should().Be(0.10m);
        pedido.Subtotal.Should().Be(7.00m);
        pedido.Desconto.Should().Be(0.70m);
        pedido.Total.Should().Be(6.30m);
    }

    [Fact]
    public void Pedido_ComApenasUmSanduiche_NaoDeveAplicarDesconto()
    {
        var pedido = new Pedido([XBurger]);

        pedido.PercentualDesconto.Should().Be(0m);
        pedido.Total.Should().Be(5.00m);
    }

    [Fact]
    public void Pedido_SemSanduiche_DeveLancarExcecao()
    {
        var acao = () => new Pedido([Batata, Refrigerante]);

        acao.Should().Throw<DomainException>()
            .WithMessage("*sanduíche*");
    }

    [Fact]
    public void Pedido_ComDoisSanduiches_DeveLancarExcecao()
    {
        var acao = () => new Pedido([XBurger, XEgg]);

        acao.Should().Throw<DomainException>()
            .WithMessage("*apenas um sanduíche*");
    }

    [Fact]
    public void Pedido_ComItemDuplicado_DeveLancarExcecao()
    {
        var acao = () => new Pedido([XBurger, XBurger]);

        acao.Should().Throw<DomainException>()
            .WithMessage("*duplicados*");
    }

    [Fact]
    public void Pedido_ComDuasBatatas_DeveLancarExcecao()
    {
        var batata2 = new ItemCardapio("BATATA2", "Batata Extra", 2.00m, TipoItem.Acompanhamento);
        var acao = () => new Pedido([XBurger, Batata, batata2]);

        acao.Should().Throw<DomainException>()
            .WithMessage("*apenas uma batata*");
    }

    [Fact]
    public void AtualizarItens_DeveRecalcularDescontoCorretamente()
    {
        var pedido = new Pedido([XBurger]);
        pedido.AtualizarItens([XBurger, Batata, Refrigerante]);

        pedido.PercentualDesconto.Should().Be(0.20m);
        pedido.AtualizadoEm.Should().NotBeNull();
    }
}
