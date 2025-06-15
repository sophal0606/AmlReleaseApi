using AmlReleaseApi.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

//// Add configuration from environment variables
//builder.Configuration.AddEnvironmentVariables();

//// Use the port provided by environment or default to 8080
//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
//builder.WebHost.UseUrls($"http://*:{port}");


// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HttpClient
builder.Services.AddHttpClient<IAmlReleaseService, AmlReleaseService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Add logging
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();