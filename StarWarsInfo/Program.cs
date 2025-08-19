using System.Text.Json;
using Microsoft.EntityFrameworkCore;
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
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
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
app.UseCors("AllowClientApp");
app.UseRouting();

// Map Controller Routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();