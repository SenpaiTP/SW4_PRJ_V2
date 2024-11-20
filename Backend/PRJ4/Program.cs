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


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };

        // Prevent default claim mapping
        options.MapInboundClaims = false;
    });
builder.Services.AddScoped<IBrugerRepo,BrugerRepo>(); // Add the BrugerRepo to the service container
builder.Services.AddScoped<ITemplateRepo<Bruger>,BrugerRepo>(); // Add the BrugerRepo to the service container
builder.Services.AddScoped<IBrugerService,BrugerService>();
builder.Services.AddScoped<IFudgifter,FudgifterRepo>();
builder.Services.AddScoped<IVudgifter,VudgifterRepo>();
builder.Services.AddScoped<IKategori,KategoriRepo>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<IFudgifterService,FudgifterService>();
builder.Services.AddScoped<IVudgifterService,VudgifterService>();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
    });

var app = builder.Build();
Console.WriteLine(builder.Configuration.GetConnectionString("DefaultConnection"));

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