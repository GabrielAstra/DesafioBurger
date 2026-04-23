namespace GoodHamburger.Application.DTOs;

public record PedidoDto(
    Guid Id,
    List<ItemCardapioDto> Itens,
    decimal Subtotal,
    decimal PercentualDesconto,
    decimal Desconto,
    decimal Total,
    DateTime CriadoEm,
    DateTime? AtualizadoEm
);
