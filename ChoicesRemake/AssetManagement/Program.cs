using AssetManagement.Services;
using KafkaService.Models;
using KafkaService.Services;
using Microsoft.Extensions.FileProviders;
using StaticAssets;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureHostConfiguration(config =>
{
    try
    {
        config.AddJsonFile("ChoicesKMS.json", optional: false);
    }
    catch
    {
        config.AddJsonFile("bin/Debug/net6.0/ChoicesKMS.json", optional: false);
    }
});
builder.Services.AddSingleton<Consumer>();
builder.Services.AddSingleton<Producer>();

var kafkaSection = builder.Configuration.GetSection(ConfigurationKeys.kafkaSection);
string brokerURL = kafkaSection[ConfigurationKeys.kafka_broker1];
string topicPrimary = kafkaSection[ConfigurationKeys.kafka_assetTopicPrimary];
string topicSecondary = kafkaSection[ConfigurationKeys.kafka_assetTopicSecondary];
string groupSecondary = kafkaSection[ConfigurationKeys.kafka_assetGroupSecondary];
string groupPrimary = kafkaSection[ConfigurationKeys.kafka_assetGroupPrimary];

builder.Services.AddHostedService<KafkaConsumer>(sp =>
{
    var producer = sp.GetRequiredService<Producer>();
    var consumer = sp.GetRequiredService<Consumer>();
    var config = new KafkaConfiguration(ctortopicPrimary: topicPrimary, ctorbrokerURL: brokerURL, ctorgroupPrimary: groupPrimary, clientName: "AssetManager", ctorgroupSecondary: groupSecondary, ctortopicSecondary: topicSecondary);
    var logger = sp.GetRequiredService<ILogger<KafkaConsumer>>();
    return new KafkaConsumer(logger, config, consumer.ManageMessage, producer.ManageMessage);
});

var app = builder.Build();
var path = Path.Combine(app.Environment.ContentRootPath, "Images");
if (!Directory.Exists(path))
    Directory.CreateDirectory(path);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/images",
    FileProvider = new PhysicalFileProvider(path)
});
app.MapControllers();

app.Run();