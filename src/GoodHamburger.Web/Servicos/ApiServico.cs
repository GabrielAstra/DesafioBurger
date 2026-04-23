using GoodHamburger.Web.Models;
using System.Net.Http.Json;

namespace GoodHamburger.Web.Servicos;

public class ApiServico(HttpClient http)
{
    public Task<List<ItemCardapioModel>?> ObterCardapio() =>
        http.GetFromJsonAsync<List<ItemCardapioModel>>("api/cardapio");

    public Task<List<PedidoModel>?> ListarPedidos() =>
        http.GetFromJsonAsync<List<PedidoModel>>("api/pedidos");

    public Task<PedidoModel?> ObterPedido(Guid id) =>
        http.GetFromJsonAsync<PedidoModel>($"api/pedidos/{id}");

    public async Task<PedidoModel?> CriarPedido(List<string> codigos)
    {
        var resposta = await http.PostAsJsonAsync("api/pedidos", new { codigosItens = codigos });
        resposta.EnsureSuccessStatusCode();
        return await resposta.Content.ReadFromJsonAsync<PedidoModel>();
    }

    public async Task<PedidoModel?> AtualizarPedido(Guid id, List<string> codigos)
    {
        var resposta = await http.PutAsJsonAsync($"api/pedidos/{id}", new { codigosItens = codigos });
        resposta.EnsureSuccessStatusCode();
        return await resposta.Content.ReadFromJsonAsync<PedidoModel>();
    }

    public async Task RemoverPedido(Guid id)
    {
        var resposta = await http.DeleteAsync($"api/pedidos/{id}");
        resposta.EnsureSuccessStatusCode();
    }
}
