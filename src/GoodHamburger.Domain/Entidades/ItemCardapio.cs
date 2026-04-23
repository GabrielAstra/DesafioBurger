using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entidades;

public class ItemCardapio
{
    public string Codigo { get; private set; }
    public string Nome { get; private set; }
    public decimal Preco { get; private set; }
    public TipoItem Tipo { get; private set; }

    public ItemCardapio(string codigo, string nome, decimal preco, TipoItem tipo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new ArgumentException("Código é obrigatório.", nameof(codigo));
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome é obrigatório.", nameof(nome));
        if (preco <= 0) throw new ArgumentException("Preço deve ser maior que zero.", nameof(preco));

        Codigo = codigo;
        Nome = nome;
        Preco = preco;
        Tipo = tipo;
    }
}
