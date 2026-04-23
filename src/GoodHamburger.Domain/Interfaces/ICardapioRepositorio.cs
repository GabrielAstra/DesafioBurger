using GoodHamburger.Domain.Entidades;

namespace GoodHamburger.Domain.Interfaces;

public interface ICardapioRepositorio
{
    IReadOnlyList<ItemCardapio> ObterTodos();
    ItemCardapio? ObterPorCodigo(string codigo);
}
