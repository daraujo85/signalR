using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configurar os serviços do SignalR
builder.Services.AddSignalR();

builder.Services.AddCors(
    options =>
        options.AddPolicy(
            "CorsPolicy",
            builder =>
            {
                builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials();
            }
        )
);

var app = builder.Build();

// Middleware para habilitar o CORS
app.UseCors("CorsPolicy");

// Definir o endpoint do hub SignalR
app.MapHub<MyHub>("/myhub");

// Endpoint para testar a conexão do hub
app.MapGet("/test", () => "SignalR Hub is running");

app.Run();

public class MyHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
