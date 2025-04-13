using System.Text.Json.Serialization;
using System.Text.Json;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using portfolium.Application.DTOs;
using portfolium.Application.Interfaces;
using portfolium.Application.Mappers;
using portfolium.Application.Services;
using portfolium.Application.Validators;
using portfolium.Core.Interfaces;
using portfolium.Infrastructure.Data;
using portfolium.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers();
builder.Services.AddControllers(options => {
    options.Filters.Add<ValidateFluentModel>();
});

builder.Services.AddValidatorsFromAssemblyContaining<StockFilterRequestValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StockRequestDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<StockUpdateRequestDtoValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockMapper, StockMapper>();
builder.Services.AddScoped<IValidator<StockRequestDto>, StockRequestDtoValidator>();

builder.Services.AddControllers()
       .AddJsonOptions(options => {
           options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
       });


var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();