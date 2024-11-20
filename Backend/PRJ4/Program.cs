using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PRJ4.Repositories;
using PRJ4.Data;
using PRJ4.Models;
using PRJ4.Infrastructure;
using PRJ4.ServiceCollectionExtension;
using PRJ4.Services;
using Serilog;
using Serilog.Events;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog for logging
var mongoConnectionString = builder.Configuration["MongoDB:connectionString"];
var mongoDatabaseName = builder.Configuration["MongoDB:databaseName"];

if (string.IsNullOrEmpty(mongoConnectionString) || string.IsNullOrEmpty(mongoDatabaseName))
{
    throw new Exception("MongoDB connection string or database name is not found in configuration or user secrets.");
}

Serilog.Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.MongoDB(
        $"{mongoConnectionString}/{mongoDatabaseName}",
        collectionName: "logs"
    )
    .WriteTo.Console()
    .CreateLogger();

// Use Serilog for logging in the host
builder.Host.UseSerilog();

// Register MongoDB client and database
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(mongoConnectionString);
});

builder.Services.AddScoped<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
});

// Add services to the container
var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddScoped<IBrugerRepo, BrugerRepo>();
builder.Services.AddScoped<ITemplateRepo<Bruger>, BrugerRepo>();
builder.Services.AddScoped<IBrugerService, BrugerService>();
builder.Services.AddScoped<IFudgifter, FudgifterRepo>();
builder.Services.AddScoped<IVudgifter, VudgifterRepo>();
builder.Services.AddScoped<IKategori, KategoriRepo>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<ILogQueryService, LogQueryService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

var app = builder.Build();
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

// Configure the HTTP request pipeline
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
