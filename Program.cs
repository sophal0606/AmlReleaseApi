using AmlReleaseApi.Services;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel to listen on port 
//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(8080); // HTTP on port 8080
//    //options.ListenAnyIP(8081); // HTTPS on port 8081
//    // Or for HTTPS:
//    // options.ListenAnyIP(8843, configure => configure.UseHttps());
//});

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