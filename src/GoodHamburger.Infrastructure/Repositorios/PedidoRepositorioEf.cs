using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Infrastructure.Contexto;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositorios;

public class PedidoRepositorioEf(GoodHamburgerDbContext db) : IPedidoRepositorio
{
    public async Task<IReadOnlyList<Pedido>> Listar() =>
        await db.Pedidos
                .OrderBy(p => p.CriadoEm)
                .ToListAsync();

    public async Task<Pedido?> ObterPorId(Guid id) =>
        await db.Pedidos.FirstOrDefaultAsync(p => p.Id == id);

    public async Task Adicionar(Pedido pedido)
    {
        db.Pedidos.Add(pedido);
        await db.SaveChangesAsync();
    }

    public async Task Atualizar(Pedido pedido)
    {
        db.Pedidos.Update(pedido);
        await db.SaveChangesAsync();
    }

    public async Task Remover(Guid id)
    {
        var pedido = await db.Pedidos.FindAsync(id);
        if (pedido is not null)
        {
            db.Pedidos.Remove(pedido);
            await db.SaveChangesAsync();
        }
    }
}
