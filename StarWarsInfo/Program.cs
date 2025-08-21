using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
});

// Approach to integrating with OAuth via Keycloak taken from
// https://stackoverflow.com/questions/77084743/secure-asp-net-core-rest-api-with-keycloak
var keycloakUrl = Environment.GetEnvironmentVariable("KEYCLOAK_URL") ?? "";
builder.Services
    .AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.MetadataAddress = $"{keycloakUrl}/realms/starwarsinfo/.well-known/openid-configuration";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RoleClaimType = "groups",
            NameClaimType = "preferred_username",
            ValidateAudience = false, //"account",
            ValidateIssuer = false, // TODO: remove if unneeded after testing
            ClockSkew = TimeSpan.FromMinutes(5)
        };
        // If you still have certificate trust issues, uncomment and adjust:
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

    });
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("email_verified", "true")
        //.RequireClaim("groups", "sw_admin", "sw_user")
        .Build();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClientApp", policy =>
    {
        policy
            .WithOrigins(
                Environment.GetEnvironmentVariable("CLIENT_APP_URL") ?? "",
                Environment.GetEnvironmentVariable("CLIENT_APP_URL_NO_PORT") ?? ""
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});
// Register HttpClient for SWAPI
builder.Services.AddHttpClient("swapi", client =>
{
    client.BaseAddress = new Uri("https://swapi.info/api/");
    client.Timeout = TimeSpan.FromSeconds(30);
    //client.DefaultRequestHeaders.UserAgent.ParseAdd("StarWarsInfo/1.0 (+https://example.com)");
});
// Register the import service
builder.Services.AddScoped<ISwapiClient, SwapiClient>();

// Configure Entity Framework Core with PostgreSQL
// Ensure you have the Npgsql.EntityFrameworkCore.PostgreSQL package installed
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline to use Controllers.
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "StarWarsInfo API v1");
        c.RoutePrefix = string.Empty; // makes Swagger UI available at "/"
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowClientApp");

// Map Controller Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();