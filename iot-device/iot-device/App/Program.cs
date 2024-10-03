using IoTDeviceApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<DeviceStateService>();
builder.Services.AddSingleton<MqttIngressService>(); // Replace with MQTT service
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Production"))
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "IoT Device API v1");
        c.RoutePrefix = string.Empty; // Access Swagger UI at root URL (http://localhost:PORT/)
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Connect to MQTT broker when the app starts
var mqttService = app.Services.GetRequiredService<MqttIngressService>();
await mqttService.ConnectAsync();

app.Lifetime.ApplicationStopping.Register(() =>
{
    mqttService.DisconnectAsync().GetAwaiter().GetResult();
});

app.Run();
