using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Infrastructure.Repositorios;

public class CardapioRepositorio : ICardapioRepositorio
{
    private static readonly IReadOnlyList<ItemCardapio> _itens =
    [
        new("X-BURGER", "X Burger",     5.00m, TipoItem.Sanduiche),
        new("X-EGG",    "X Egg",        4.50m, TipoItem.Sanduiche),
        new("X-BACON",  "X Bacon",      7.00m, TipoItem.Sanduiche),
        new("BATATA",   "Batata Frita", 2.00m, TipoItem.Acompanhamento),
        new("REFRI",    "Refrigerante", 2.50m, TipoItem.Bebida),
    ];

    public IReadOnlyList<ItemCardapio> ObterTodos() => _itens;

    public ItemCardapio? ObterPorCodigo(string codigo) =>
        _itens.FirstOrDefault(i => i.Codigo.Equals(codigo, StringComparison.OrdinalIgnoreCase));
}
