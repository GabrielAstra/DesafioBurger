using GoodHamburger.Domain.Excecoes;
using System.Text.Json;

namespace GoodHamburger.API.Middlewares;

public class TratadorDeExcecoesMiddleware(RequestDelegate proximo, ILogger<TratadorDeExcecoesMiddleware> logger)
{
    private static readonly JsonSerializerOptions _jsonOpcoes = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await proximo(contexto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro não tratado: {Mensagem}", ex.Message);
            await EscreverRespostaDeErro(contexto, ex);
        }
    }

    private static Task EscreverRespostaDeErro(HttpContext contexto, Exception ex)
    {
        var (statusCode, mensagem) = ex switch
        {
            RecursoNaoEncontradoException => (StatusCodes.Status404NotFound, ex.Message),
            DomainException               => (StatusCodes.Status422UnprocessableEntity, ex.Message),
            _                             => (StatusCodes.Status500InternalServerError, "Ocorreu um erro interno. Tente novamente mais tarde.")
        };

        contexto.Response.ContentType = "application/json";
        contexto.Response.StatusCode = statusCode;

        var corpo = JsonSerializer.Serialize(new { erro = mensagem }, _jsonOpcoes);
        return contexto.Response.WriteAsync(corpo);
    }
}
