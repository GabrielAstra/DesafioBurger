namespace GoodHamburger.Web.Models;

public class PedidoModel
{
    public Guid Id { get; set; }
    public List<ItemCardapioModel> Itens { get; set; } = [];
    public decimal Subtotal { get; set; }
    public decimal PercentualDesconto { get; set; }
    public decimal Desconto { get; set; }
    public decimal Total { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
