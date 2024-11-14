
using ConsoleApp1.Services;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

ConfigurationManager configuration = builder.Configuration;
configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

builder.Services.AddHostedService<MainService>();

builder.Services.AddInfrastructureServices(configuration);

var app = builder.Build();

app.Run();