using GoodHamburger.Domain.Entidades;

namespace GoodHamburger.Domain.Interfaces;

public interface IPedidoRepositorio
{
    Task<IReadOnlyList<Pedido>> Listar();
    Task<Pedido?> ObterPorId(Guid id);
    Task Adicionar(Pedido pedido);
    Task Atualizar(Pedido pedido);
    Task Remover(Guid id);
}
