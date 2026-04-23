using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

[ApiController]
[Route("api/cardapio")]
[Produces("application/json")]
public class CardapioController(ICardapioServico servico) : ControllerBase
{
    [HttpGet]
    public IActionResult ObterCardapio() => Ok(servico.ObterCardapio());
}
