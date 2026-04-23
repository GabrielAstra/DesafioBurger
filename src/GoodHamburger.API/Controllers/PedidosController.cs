using GoodHamburger.Application.DTOs;
using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/pedidos")]
[Produces("application/json")]
public class PedidosController(IPedidoServico servico) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar() => Ok(await servico.Listar());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id) =>
        Ok(await servico.ObterPorId(id));

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarPedidoDto dto)
    {
        var pedido = await servico.Criar(dto);
        return CreatedAtAction(nameof(ObterPorId), new { id = pedido.Id }, pedido);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Atualizar(Guid id, [FromBody] CriarPedidoDto dto) =>
        Ok(await servico.Atualizar(id, dto));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Remover(Guid id)
    {
        await servico.Remover(id);
        return NoContent();
    }
}
