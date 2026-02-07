using TestCRM.BLL.Interfaces;
using TestCRM.BLL.Services;
using TestCRM.DAL.Data;
using TestCRM.DAL.Interfaces;
using TestCRM.DAL.Repositories;
using TestCRM.PL.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<ILeadRepository, LeadRepository>();

builder.Services.AddSingleton<ILeadQueue, LeadQueue>();
builder.Services.AddScoped<ILeadProcessor, LeadProcessor>();

builder.Services.AddHostedService<LeadProcessingWorker>();
//builder.Services.AddHostedService<MockLeadIngestionService>();

builder.Host.UseWindowsService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
