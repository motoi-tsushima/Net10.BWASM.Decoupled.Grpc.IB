using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using IssueBoard.Grpc;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Net10.BWASM.Decoupled.Grpc.IB.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var grpcApiBaseUrl = builder.Configuration["GrpcApiBaseUrl"] ?? "https://localhost:7116";

builder.Services.AddScoped(_ =>
{
    var handler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
    var channel = GrpcChannel.ForAddress(grpcApiBaseUrl, new GrpcChannelOptions
    {
        HttpHandler = handler
    });
    return new IssueBoardService.IssueBoardServiceClient(channel);
});

builder.Services.AddScoped<LocalStorageService>();

await builder.Build().RunAsync();
