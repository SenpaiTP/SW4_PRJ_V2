using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Serilog.Events;
using MongoDB.Driver;
using PRJ4.Repositories;
using PRJ4.Data;
using PRJ4.Models;
using PRJ4.Services;
using PRJ4.Infrastructure;
using PRJ4.ServiceCollectionExtension;
using PRJ4.Mappings;
using Serilog;
using Serilog.Events;
using MongoDB.Driver;
using PRJ4.Services;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using DnsClient.Protocol;



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
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)  // This reads from appsettings.json or user-secrets
    .WriteTo.MongoDB(
        builder.Configuration["MongoDB:connectionString"] + "/" + builder.Configuration["MongoDB:databaseName"],
        collectionName: "logs"
        )
    .WriteTo.Console() // Use connection string from configuration
    .CreateLogger();

// Use Serilog for logging in the host
builder.Host.UseSerilog();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGenWithAuth();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

// Register MongoDB client and database
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(mongoConnectionString);
});

builder.Services.AddIdentity<ApiUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 2;
        })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
    o.TokenLifespan = TimeSpan.FromHours(2));
builder.Services.AddAuthentication(options=>
    {
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme = 
    options.DefaultForbidScheme =
    options.DefaultScheme=
    options.DefaultSignInScheme=
    options.DefaultSignOutScheme= JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>{
        options.TokenValidationParameters=new TokenValidationParameters
        {
            ValidateIssuer=true,
            ValidIssuer=builder.Configuration["JWT:Issuer"],
            ValidateAudience=true,
            ValidAudience = builder.Configuration["JWT:Audience"],
            ValidateIssuerSigningKey=true,
            IssuerSigningKey=new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JWT:SigningKey"])),
        };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var token =context.SecurityToken as JwtSecurityToken;
            if (token !=null)
            {
                var TokenId=token.RawData;
                var revocationService=context.HttpContext.RequestServices.GetRequiredService<IRevocationService>();
                var tokenIsRevoked=await revocationService.IsTokenRevokedAsync(TokenId);

                if(tokenIsRevoked)
                {
                    context.Fail("Token is revoked");
                }
            }
            // var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
            // foreach (var claim in claims)
            // {
            //     Console.WriteLine(claim);  // Log claims to verify them
            // }
            // return Task.CompletedTask;
        }
    };
    });

builder.Services.AddScoped<IMongoDatabase>(serviceProvider =>
{
    var client = serviceProvider.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
});

<<<<<<< HEAD
=======

>>>>>>> main
//Register mapping profiles
//builder.Services.AddAutoMapper(typeof(FudgifterProfile));
//builder.Services.AddAutoMapper(typeof(VudgifterProfile));
builder.Services.AddAutoMapper(typeof(LogMappingProfile));
// Add services to the container
var conn = builder.Configuration["ConnectionStrings:DefaultConnection"];

builder.Services.AddAuthorization();

builder.Services.AddScoped<IBrugerRepo, BrugerRepo>();
builder.Services.AddScoped<ITemplateRepo<Bruger>, BrugerRepo>();
//builder.Services.AddScoped<IBrugerService, BrugerService>();
builder.Services.AddScoped<IFindtægtRepo, FindtægtRepo>();
builder.Services.AddScoped<IVindtægtRepo, VindtægtRepo>();
//builder.Services.AddScoped<IFudgifter, FudgifterRepo>();
//builder.Services.AddScoped<IVudgifter, VudgifterRepo>();
builder.Services.AddScoped<IKategoriRepo, KategoriRepo>();
//builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<IFindtægtService, FindtægtService>();
builder.Services.AddScoped<IVindtægtService, VindtægtService>();
//builder.Services.AddScoped<IFudgifterService,FudgifterService>();
//builder.Services.AddScoped<IVudgifterService,VudgifterService>();

builder.Services.AddScoped<IFudgifterRepo, FudgifterRepo>();

//Build Budgets
builder.Services.AddScoped<IBudgetRepo,BudgetRepo>();
builder.Services.AddScoped<ITemplateRepo<Budget>,BudgetRepo>();
builder.Services.AddScoped<IBudgetGoalService,BudgetGoalService>();
builder.Services.AddScoped<IRevocationService,RevocationService>();

//Build Kategory Limit
builder.Services.AddScoped<IKategoryLimitRepo,KategoryLimitRepo>();
builder.Services.AddScoped<ITemplateRepo<KategoryLimit>,KategoryLimitRepo>();
builder.Services.AddScoped<IKategoryLimitService,KategoryLimitService>();

builder.Services.AddScoped<IVudgifterRepo, VudgifterRepo>();
builder.Services.AddScoped<IKategoriRepo, KategoriRepo>();
//builder.Services.AddScoped<TokenProvider>();
builder.Services.AddScoped<IVindtægtService, VindtægtService>();    
builder.Services.AddScoped<IFudgifterService,FudgifterService>();
builder.Services.AddScoped<IVudgifterService,VudgifterService>();
builder.Services.AddScoped<ILogQueryService, LogQueryService>();
builder.Services.AddSingleton(provider =>
    new EmailService(
        "3f60840382a09172c229cac33ddd7e63-f55d7446-6c90d163", // Replace with your Mailgun API key
        "sandboxf55113ec9eef4f6580a316b419167ded.mailgun.org" // Replace with your Mailgun domain
    )
);
builder.Services.AddControllers();
string openAiKey = builder.Configuration["OpenAI:ApiKey"];
builder.Services.AddSingleton(new OpenAIClient(openAiKey));



builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]);
});

var app = builder.Build();


// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
    