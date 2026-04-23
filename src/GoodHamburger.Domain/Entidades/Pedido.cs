using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Excecoes;

namespace GoodHamburger.Domain.Entidades;

public class Pedido
{
    private readonly List<ItemCardapio> _itens = [];

    public Guid Id { get; private set; }
    public IReadOnlyList<ItemCardapio> Itens => _itens.AsReadOnly();
    public decimal Subtotal => _itens.Sum(i => i.Preco);
    public decimal PercentualDesconto { get; private set; }
    public decimal Desconto => Math.Round(Subtotal * PercentualDesconto, 2);
    public decimal Total => Math.Round(Subtotal - Desconto, 2);
    public DateTime CriadoEm { get; private set; }
    public DateTime? AtualizadoEm { get; private set; }

    private Pedido() { }

    public Pedido(IEnumerable<ItemCardapio> itens)
    {
        Id = Guid.NewGuid();
        CriadoEm = DateTime.UtcNow;
        AdicionarItens(itens);
    }

    public void AtualizarItens(IEnumerable<ItemCardapio> novosItens)
    {
        _itens.Clear();
        AdicionarItens(novosItens);
        AtualizadoEm = DateTime.UtcNow;
    }

    private void AdicionarItens(IEnumerable<ItemCardapio> itens)
    {
        var lista = itens.ToList();

        ValidarDuplicatas(lista);
        ValidarQuantidadePorTipo(lista);

        _itens.AddRange(lista);
        PercentualDesconto = CalcularDesconto();
    }

    private static void ValidarDuplicatas(List<ItemCardapio> itens)
    {
        var duplicatas = itens
            .GroupBy(i => i.Codigo)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicatas.Count > 0)
            throw new DomainException($"Itens duplicados no pedido: {string.Join(", ", duplicatas)}.");
    }

    private static void ValidarQuantidadePorTipo(List<ItemCardapio> itens)
    {
        var sanduiches = itens.Count(i => i.Tipo == TipoItem.Sanduiche);
        var acompanhamentos = itens.Count(i => i.Tipo == TipoItem.Acompanhamento);
        var bebidas = itens.Count(i => i.Tipo == TipoItem.Bebida);

        if (sanduiches == 0)
            throw new DomainException("O pedido deve conter pelo menos um sanduíche.");
        if (sanduiches > 1)
            throw new DomainException("O pedido pode conter apenas um sanduíche.");
        if (acompanhamentos > 1)
            throw new DomainException("O pedido pode conter apenas uma batata frita.");
        if (bebidas > 1)
            throw new DomainException("O pedido pode conter apenas um refrigerante.");
    }

    private decimal CalcularDesconto()
    {
        var temSanduiche = _itens.Any(i => i.Tipo == TipoItem.Sanduiche);
        var temAcompanhamento = _itens.Any(i => i.Tipo == TipoItem.Acompanhamento);
        var temBebida = _itens.Any(i => i.Tipo == TipoItem.Bebida);

        if (temSanduiche && temAcompanhamento && temBebida) return 0.20m;
        if (temSanduiche && temBebida) return 0.15m;
        if (temSanduiche && temAcompanhamento) return 0.10m;

        return 0m;
    }
}
