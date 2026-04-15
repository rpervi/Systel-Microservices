using BFFService.Application;
using BFFService.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
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

var serviceUrls = builder.Configuration.GetSection("ServiceUrls");

// 1. Get the URL from configuration once
var masterServiceUrl = serviceUrls["MasterService"];

if (string.IsNullOrEmpty(serviceUrls["MasterService"]))
{
    // High-priority check: prevent the app from starting with a broken config
    throw new Exception("CRITICAL: MasterService URL is missing in appsettings.json");
}

// 2. Register the client once with all necessary logic
builder.Services.AddHttpClient("PartServiceClient", client =>
{
    if (string.IsNullOrEmpty(masterServiceUrl))
        throw new Exception("CRITICAL: MasterService URL is missing.");

    // Ensure trailing slash for consistent URI combining
    client.BaseAddress = new Uri(masterServiceUrl.EndsWith("/") ? masterServiceUrl : serviceUrls["MasterService"] + "/");

    // Optional: Add a default timeout (good architect practice)
    client.Timeout = TimeSpan.FromSeconds(30);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // SSL Bypass: Necessary for local HTTPS certificates between services
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
});


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "BFFService.Api", Version = "v1" });

    // 1. Define the "Bearer" Security Scheme
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    // 2. Make sure Swagger uses that scheme for every request
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
// Add these BEFORE builder.Build()
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IServiceProxy, ServiceProxy>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
