using TestCRM.BLL.Interfaces;
using TestCRM.BLL.Services;
using TestCRM.DAL.Data;
using TestCRM.DAL.Interfaces;
using TestCRM.DAL.Repositories;
using TestCRM.PL.Service;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<DapperContext>();
        services.AddScoped<ILeadRepository, LeadRepository>();

        services.AddSingleton<ILeadQueue, LeadQueue>();
        services.AddScoped<ILeadProcessor, LeadProcessor>();

        services.AddHostedService<Worker>();
    })
    .UseWindowsService();

var host = builder.Build();
host.Run();
