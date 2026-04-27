using CompanyService.Application.Interfaces;
using CompanyService.Application.Services;
using CompanyService.Domain.Interfaces;
using CompanyService.Infrastructure.Data;
using CompanyService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 1. Configure PostgreSQL Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CompanyDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Register Dependency Injection (The "Electrician" wiring the layers)
// Mapping the Domain Interface to the Infrastructure Implementation
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

// Mapping the Application Interface to the Application Implementation
builder.Services.AddScoped<ICompanyService, CompanyServiceImpl>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
