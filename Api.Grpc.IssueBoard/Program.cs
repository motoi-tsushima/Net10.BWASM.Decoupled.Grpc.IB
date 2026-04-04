using Api.Grpc.IssueBoard.Data;
using Api.Grpc.IssueBoard.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddDbContext<IssuesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorClient", policy =>
    {
        var origins = builder.Configuration["AllowedOrigins"]?.Split(',')
            ?? ["https://localhost:7131"];
        policy
            .WithOrigins([.. origins, "http://localhost:5096"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders(
                "Grpc-Status",
                "Grpc-Message",
                "Grpc-Encoding",
                "Grpc-Accept-Encoding");
    });
});

var app = builder.Build();

app.UseCors("BlazorClient");
app.UseGrpcWeb();

app.MapGrpcService<IssueBoardGrpcService>().EnableGrpcWeb().RequireCors("BlazorClient");

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
