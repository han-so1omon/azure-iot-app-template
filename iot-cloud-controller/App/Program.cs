using YourNamespace;
using YourNamespace.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<SystemLogsService>();
builder.Services.AddHostedService<KafkaConsumerService>(); // Register the Kafka consumer service
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "IoT Cloud Controller API v1");
});
/*
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
