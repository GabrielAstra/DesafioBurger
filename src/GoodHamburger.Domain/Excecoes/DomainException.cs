namespace GoodHamburger.Domain.Excecoes;

public class DomainException : Exception
{
    public DomainException(string mensagem) : base(mensagem) { }
}
