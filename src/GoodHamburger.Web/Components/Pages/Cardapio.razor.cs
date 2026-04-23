using GoodHamburger.Web.Models;
using GoodHamburger.Web.Servicos;
using Microsoft.AspNetCore.Components;

namespace GoodHamburger.Web.Components.Pages;

public class CardapioBase : ComponentBase
{
    [Inject] protected ApiServico Api { get; set; } = default!;

    protected List<ItemCardapioModel> Itens { get; private set; } = [];
    protected bool Carregando { get; private set; } = true;
    protected string? Erro { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Itens = await Api.ObterCardapio() ?? [];
        }
        catch (Exception ex)
        {
            Erro = $"Não foi possível carregar o cardápio: {ex.Message}";
        }
        finally
        {
            Carregando = false;
        }
    }
}
