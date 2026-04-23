using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Excecoes;
using GoodHamburger.Domain.Interfaces;

namespace GoodHamburger.Application.Servicos;

public class PedidoServico(
    IPedidoRepositorio pedidoRepositorio,
    ICardapioRepositorio cardapioRepositorio) : IPedidoServico
{
    public async Task<IReadOnlyList<PedidoDto>> Listar()
    {
        var pedidos = await pedidoRepositorio.Listar();
        return pedidos.Select(MapearParaDto).ToList();
    }

    public async Task<PedidoDto> ObterPorId(Guid id)
    {
        var pedido = await pedidoRepositorio.ObterPorId(id)
            ?? throw new RecursoNaoEncontradoException($"Pedido '{id}' não encontrado.");

        return MapearParaDto(pedido);
    }

    public async Task<PedidoDto> Criar(CriarPedidoDto dto)
    {
        var itens = ResolverItens(dto.CodigosItens);
        var pedido = new Pedido(itens);
        await pedidoRepositorio.Adicionar(pedido);
        return MapearParaDto(pedido);
    }

    public async Task<PedidoDto> Atualizar(Guid id, CriarPedidoDto dto)
    {
        var pedido = await pedidoRepositorio.ObterPorId(id)
            ?? throw new RecursoNaoEncontradoException($"Pedido '{id}' não encontrado.");

        var itens = ResolverItens(dto.CodigosItens);
        pedido.AtualizarItens(itens);
        await pedidoRepositorio.Atualizar(pedido);
        return MapearParaDto(pedido);
    }

    public async Task Remover(Guid id)
    {
        var pedido = await pedidoRepositorio.ObterPorId(id)
            ?? throw new RecursoNaoEncontradoException($"Pedido '{id}' não encontrado.");

        await pedidoRepositorio.Remover(pedido.Id);
    }

    private List<ItemCardapio> ResolverItens(List<string> codigos)
    {
        var itens = new List<ItemCardapio>();

        foreach (var codigo in codigos)
        {
            var item = cardapioRepositorio.ObterPorCodigo(codigo)
                ?? throw new DomainException($"Item '{codigo}' não encontrado no cardápio.");

            itens.Add(item);
        }

        return itens;
    }

    private static PedidoDto MapearParaDto(Pedido pedido) => new(
        pedido.Id,
        pedido.Itens.Select(i => new ItemCardapioDto(i.Codigo, i.Nome, i.Preco, i.Tipo.ToString())).ToList(),
        pedido.Subtotal,
        pedido.PercentualDesconto,
        pedido.Desconto,
        pedido.Total,
        pedido.CriadoEm,
        pedido.AtualizadoEm
    );
}
