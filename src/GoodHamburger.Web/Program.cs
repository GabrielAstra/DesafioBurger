using GoodHamburger.Web.Components;
using GoodHamburger.Web.Servicos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HttpClient apontando para a API
var apiUrl = builder.Configuration["ApiUrl"] ?? "https://localhost:7250/";
builder.Services.AddHttpClient<ApiServico>(c => c.BaseAddress = new Uri(apiUrl));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
