using MasterService.Application.Interfaces;
using MasterService.Infrastructure.Data;
using MasterService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Add this
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; // Add this
using Microsoft.OpenApi.Models;
using Systel.Shared.Exceptions;
using Systel.Shared.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// --- 1. ADD JWT AUTHENTICATION (Must match BFF settings) ---
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Systel MasterService", Version = "v1" });

    // 1. Define the "Bearer" security scheme
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    // 2. Make Swagger use the "Bearer" scheme for all operations
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddDbContext<MasterDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPartRepository, PartRepository>();

// Register from Shared Library
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

// 1. Set the Base Path for the entire application (Optional but recommended)
app.UsePathBase("/MasterService");

app.UseRouting(); // Optional, but good practice before Auth

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        // 2. Change the endpoint to include the prefix
        //options.SwaggerEndpoint("/MasterService/swagger/v1/swagger.json", "MasterService API V1");
        options.SwaggerEndpoint("v1/swagger.json", "MasterService API V1");

        // 3. This ensures you can access it at /MasterService/swagger
        //options.RoutePrefix = "MasterService/swagger";
        options.RoutePrefix = "swagger";
    });
}

// --- STEP 4: ERROR HANDLING & SECURITY ---
app.UseExceptionHandler();

// Comment this out or wrap it in a check
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

//app.UseExceptionHandler();
//app.UseMiddleware<RequestLoggingMiddleware>();
//app.UseHttpsRedirection();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
