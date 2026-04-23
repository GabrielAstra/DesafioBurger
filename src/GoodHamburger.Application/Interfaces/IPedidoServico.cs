using GoodHamburger.Application.DTOs;

namespace GoodHamburger.Application.Interfaces;

public interface IPedidoServico
{
    Task<IReadOnlyList<PedidoDto>> Listar();
    Task<PedidoDto> ObterPorId(Guid id);
    Task<PedidoDto> Criar(CriarPedidoDto dto);
    Task<PedidoDto> Atualizar(Guid id, CriarPedidoDto dto);
    Task Remover(Guid id);
}
