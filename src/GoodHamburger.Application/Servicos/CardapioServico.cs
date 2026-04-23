using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.Servicos;

public class CardapioServico(ICardapioRepositorio repositorio) : ICardapioServico
{
    public IReadOnlyList<ItemCardapioDto> ObterCardapio() =>
        repositorio.ObterTodos()
            .Select(i => new ItemCardapioDto(i.Codigo, i.Nome, i.Preco, i.Tipo.ToString()))
            .ToList();
}
