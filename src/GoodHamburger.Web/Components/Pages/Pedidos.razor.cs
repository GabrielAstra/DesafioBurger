using GoodHamburger.Web.Models;
using GoodHamburger.Web.Servicos;
using Microsoft.AspNetCore.Components;

namespace GoodHamburger.Web.Components.Pages;

public class PedidosBase : ComponentBase
{
    [Inject] protected ApiServico Api { get; set; } = default!;

    protected List<PedidoModel> Pedidos { get; private set; } = [];
    protected List<ItemCardapioModel> Cardapio { get; private set; } = [];
    protected List<string> CodigosSelecionados { get; private set; } = [];
    protected PedidoModel? PedidoEditando { get; private set; }
    protected bool FormularioAberto { get; private set; }
    protected bool Carregando { get; private set; } = true;
    protected bool Salvando { get; private set; }
    protected string? MensagemErro { get; private set; }
    protected string? MensagemSucesso { get; private set; }
    protected Guid? IdParaRemover { get; private set; }

    protected decimal Subtotal => Cardapio
        .Where(i => CodigosSelecionados.Contains(i.Codigo))
        .Sum(i => i.Preco);

    protected decimal PercentualDesconto
    {
        get
        {
            var temSanduiche = Cardapio.Any(i => CodigosSelecionados.Contains(i.Codigo) && i.Tipo == "Sanduiche");
            var temBatata    = Cardapio.Any(i => CodigosSelecionados.Contains(i.Codigo) && i.Tipo == "Acompanhamento");
            var temRefri     = Cardapio.Any(i => CodigosSelecionados.Contains(i.Codigo) && i.Tipo == "Bebida");

            if (temSanduiche && temBatata && temRefri) return 0.20m;
            if (temSanduiche && temRefri)              return 0.15m;
            if (temSanduiche && temBatata)             return 0.10m;
            return 0m;
        }
    }

    protected decimal Desconto => Math.Round(Subtotal * PercentualDesconto, 2);
    protected decimal Total    => Math.Round(Subtotal - Desconto, 2);
    protected bool PodeSalvar  => CodigosSelecionados.Any(c =>
        Cardapio.Any(i => i.Codigo == c && i.Tipo == "Sanduiche"));

    protected override async Task OnInitializedAsync()
    {
        await Task.WhenAll(CarregarCardapio(), CarregarPedidos());
        Carregando = false;
    }

    private async Task CarregarCardapio() => Cardapio = await Api.ObterCardapio() ?? [];
    private async Task CarregarPedidos()  => Pedidos  = await Api.ListarPedidos() ?? [];

    protected void AbrirFormularioNovo()
    {
        PedidoEditando      = null;
        CodigosSelecionados = [];
        MensagemErro        = null;
        MensagemSucesso     = null;
        FormularioAberto    = true;
    }

    protected void AbrirFormularioEdicao(PedidoModel pedido)
    {
        PedidoEditando      = pedido;
        CodigosSelecionados = pedido.Itens.Select(i => i.Codigo).ToList();
        MensagemErro        = null;
        MensagemSucesso     = null;
        FormularioAberto    = true;
    }

    protected void FecharFormulario()
    {
        FormularioAberto    = false;
        PedidoEditando      = null;
        CodigosSelecionados = [];
    }

    protected void ToggleItem(ItemCardapioModel item)
    {
        if (CodigosSelecionados.Contains(item.Codigo))
            CodigosSelecionados.Remove(item.Codigo);
        else if (!ItemDesabilitado(item))
            CodigosSelecionados.Add(item.Codigo);
    }

    protected bool ItemDesabilitado(ItemCardapioModel item) =>
        Cardapio.Any(i => i.Tipo == item.Tipo && CodigosSelecionados.Contains(i.Codigo));

    protected async Task SalvarPedido()
    {
        Salvando        = true;
        MensagemErro    = null;
        MensagemSucesso = null;

        try
        {
            if (PedidoEditando is null)
            {
                var novo = await Api.CriarPedido(CodigosSelecionados);
                if (novo is not null) Pedidos.Insert(0, novo);
                MensagemSucesso = "Pedido criado com sucesso!";
            }
            else
            {
                var atualizado = await Api.AtualizarPedido(PedidoEditando.Id, CodigosSelecionados);
                if (atualizado is not null)
                {
                    var idx = Pedidos.FindIndex(p => p.Id == atualizado.Id);
                    if (idx >= 0) Pedidos[idx] = atualizado;
                }
                MensagemSucesso = "Pedido atualizado com sucesso!";
            }

            FecharFormulario();
        }
        catch (HttpRequestException ex)
        {
            MensagemErro = $"Erro: {ex.Message}";
        }
        finally
        {
            Salvando = false;
        }
    }

    protected void ConfirmarRemocao(Guid id)
    {
        IdParaRemover   = id;
        MensagemErro    = null;
        MensagemSucesso = null;
    }

    protected async Task ExecutarRemocao()
    {
        if (IdParaRemover is null) return;

        try
        {
            await Api.RemoverPedido(IdParaRemover.Value);
            Pedidos.RemoveAll(p => p.Id == IdParaRemover.Value);
            MensagemSucesso = "Pedido removido.";
        }
        catch (HttpRequestException ex)
        {
            MensagemErro = $"Erro ao remover: {ex.Message}";
        }
        finally
        {
            IdParaRemover = null;
        }
    }

    protected void CancelarRemocao() => IdParaRemover = null;
}
