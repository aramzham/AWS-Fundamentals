using Weather.Api.Models;
using Weather.Api.Services;
using Weather.Api.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var env = builder.Environment.EnvironmentName;
var appName = builder.Environment.ApplicationName;
builder.Configuration.AddSecretsManager(configurator: options =>
{
    options.SecretFilter = entry => entry.Name.StartsWith($"{env}_{appName}");
    options.KeyGenerator = (entry, s) => s.Replace($"{env}_{appName}_", string.Empty)
        .Replace("__", ":");
    // set this for secrets rotation
    options.PollingInterval = TimeSpan.FromSeconds(20); // don't set the interval for so low because you'll be paying for each request (usually it may be like every hour or half an hor)
}); // this will dynamically set configuration in appsettings.json from aws_secrets_manager, of course with our predefined secrets naming convention "{env}_{appName}_{jsonProperty}__{key}"

builder.Services.AddTransient<IWeatherService, WeatherService>();
builder.Services.AddHttpClient();
builder.Services.Configure<OpenWeatherApiSettings>(builder.Configuration.GetSection("OpenWeatherMapApi"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();