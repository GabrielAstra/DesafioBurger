using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Application.DTOs;

public record CriarPedidoDto(
    [Required(ErrorMessage = "A lista de itens é obrigatória.")]
    [MinLength(1, ErrorMessage = "O pedido deve conter pelo menos um item.")]
    List<string> CodigosItens
);
