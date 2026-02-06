using TestCRM.DAL.Data;
using TestCRM.PL.Service;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton<DapperContext>();

var host = builder.Build();
host.Run();
