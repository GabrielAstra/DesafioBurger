using GoodHamburger.Domain.Entidades;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Contexto;

public class GoodHamburgerDbContext(DbContextOptions<GoodHamburgerDbContext> options) : DbContext(options)
{
    public DbSet<Pedido> Pedidos => Set<Pedido>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GoodHamburgerDbContext).Assembly);
    }
}
