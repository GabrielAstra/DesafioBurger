using GoodHamburger.Domain.Entidades;
using GoodHamburger.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoodHamburger.Infrastructure.Configuracoes;

public class PedidoConfiguracao : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("Pedidos");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.PercentualDesconto)
               .HasColumnType("decimal(5,4)");

        builder.Property(p => p.CriadoEm);
        builder.Property(p => p.AtualizadoEm);

        builder.Ignore(p => p.Subtotal);
        builder.Ignore(p => p.Desconto);
        builder.Ignore(p => p.Total);
        builder.Ignore(p => p.Itens);

        builder.OwnsMany<ItemCardapio>("_itens", itens =>
        {
            itens.ToTable("ItensPedido");
            itens.WithOwner().HasForeignKey("PedidoId");
            itens.HasKey("PedidoId", "Codigo");

            itens.Property(i => i.Codigo).HasMaxLength(20);
            itens.Property(i => i.Nome).HasMaxLength(100);
            itens.Property(i => i.Preco).HasColumnType("decimal(10,2)");
            itens.Property(i => i.Tipo).HasConversion<string>();
        });
    }
}
