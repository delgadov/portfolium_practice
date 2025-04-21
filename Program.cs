using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using portfolium.Application.Interfaces;
using portfolium.Application.Mappers;
using portfolium.Application.Services;
using portfolium.Application.Validators;
using portfolium.Core.Configuration;
using portfolium.Core.Constants;
using portfolium.Core.Interfaces;
using portfolium.Infrastructure.Data;
using portfolium.Infrastructure.Health;
using portfolium.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.Configure<HealthCheckSettings>(builder.Configuration.GetSection("HealthChecks"));
builder.Services.AddHealthChecksService(builder.Configuration);
builder.Services.AddHealthCheckUiService(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers(options => { options.Filters.Add<ValidateFluentModel>(); })
       .AddNewtonsoftJson()
       .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });


builder.Services.AddValidatorsFromAssemblyContaining<StockFilterRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StockRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StockUpdateRequestDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockMapper, StockMapper>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthCheckEndpoint();
app.MapHealthCheckUiEndpoints();
app.MapControllers();

app.Run();