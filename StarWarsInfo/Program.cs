using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using StarWarsInfo.Data;
using StarWarsInfo.Integrations.Swapi;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console() // STDOUT
    .CreateLogger();

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
        options.RequireHttpsMetadata = false; // Set to false for self-signed certs
        options.MetadataAddress = $"{keycloakUrl}/realms/starwarsinfo/.well-known/openid-configuration";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // "groups" is actually an array of Realm Roles from Keycloak. Create the Realm Roles
            // "sw_admin" and "sw_user", and then in the Keycloak client add the "groups" mapping
            // under Client scopes -> (client name)-dedicated -> Add mapper -> predefined -> groups
            RoleClaimType = "groups",
            NameClaimType = "preferred_username",
            // "account" is the default client ID from Keycloak that is used in the aud payload
            ValidAudience = "account",
            // https://stackoverflow.com/questions/60306175/bearer-error-invalid-token-error-description-the-issuer-is-invalid
            ValidateIssuer = false, // TODO: remove if unneeded after testing
            ClockSkew = TimeSpan.FromMinutes(5)
        };
        // Needed for self-signed certs
        options.BackchannelHttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        // Helpful logging
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = ctx =>
            {
                Console.WriteLine($"JWT auth failed: {ctx.Exception}");
                return Task.CompletedTask;
            },
            OnChallenge = ctx =>
            {
                Console.WriteLine($"JWT challenge: {ctx.Error} - {ctx.ErrorDescription}");
                return Task.CompletedTask;
            }
        };
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
            .AllowCredentials()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .RequireClaim("email_verified", "true")
        // Required Realm Roles from Keycloak
        .RequireClaim("groups", "sw_admin", "sw_user")
        .Build();
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
builder.Services.AddScoped<ISwapiImportService, SwapiImportService>();

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
app.UseCors("AllowClientApp");
app.UseAuthentication();
app.UseAuthorization();

app.Run();