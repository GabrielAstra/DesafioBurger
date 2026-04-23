using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface ICardapioServico
{
    IReadOnlyList<ItemCardapioDto> ObterCardapio();
}
