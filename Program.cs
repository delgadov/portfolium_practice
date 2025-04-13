using System.Text.Json.Serialization;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using portfolium.Application.DTOs;
using portfolium.Application.Interfaces;
using portfolium.Application.Mappers;
using portfolium.Application.Services;
using portfolium.Application.Validators;
using portfolium.Core.Interfaces;
using portfolium.Infrastructure.Data;
using portfolium.Infrastructure.Repositories;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key not found")))
        };
    });

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddControllers();
builder.Services.AddControllers(options => { options.Filters.Add<ValidateFluentModel>(); });

builder.Services.AddValidatorsFromAssemblyContaining<StockFilterRequestValidator>();
builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockMapper, StockMapper>();
builder.Services.AddScoped<IValidator<StockRequestDto>, StockRequestDtoValidator>();

builder.Services.AddControllers()
       .AddJsonOptions(options =>
           options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add security headers
app.Use(async (context, next) => {
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("Content-Security-Policy", 
        "default-src 'self'; " +
        "frame-ancestors 'none'; " +
        "img-src 'self' data: https:; " +
        "style-src 'self' 'unsafe-inline' https:; " +
        "script-src 'self' 'unsafe-inline' 'unsafe-eval' https:;");
    await next();
});

// Add rate limiting
app.UseRateLimiter(new RateLimiterOptions {
    GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context => {
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            });
    })
});

app.UseHttpsRedirection();

// Add authentication & authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();