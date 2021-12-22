using AssetManagement.Services;
using KafkaService.Models;
using KafkaService.Services;
using Microsoft.Extensions.FileProviders;
using StaticAssets;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
});
builder.Host.ConfigureHostConfiguration(config =>
{
    /*try
    {
        config.AddJsonFile("ChoicesKMS.json", optional: false);
    }
    catch
    {
        config.AddJsonFile("bin/Debug/net6.0/ChoicesKMS.json", optional: false);
    }*/

    config.AddEnvironmentVariables(prefix: "CR_");

});
builder.Services.AddSingleton<Consumer>();
builder.Services.AddSingleton<Producer>();

string brokerURL = builder.Configuration.GetValue<string>(ConfigurationKeys.kafka_broker1);
string topicPrimary = builder.Configuration.GetValue<string>(ConfigurationKeys.kafka_assetTopicPrimary);
string topicSecondary = builder.Configuration.GetValue<string>(ConfigurationKeys.kafka_assetTopicSecondary);
string groupSecondary = builder.Configuration.GetValue<string>(ConfigurationKeys.kafka_assetGroupSecondary);
string groupPrimary = builder.Configuration.GetValue<string>(ConfigurationKeys.kafka_assetGroupPrimary);
string[] ipParts = brokerURL.Split(':');
var currentIP=Dns.GetHostEntry(ipParts[0]);
var stringIP=string.Concat(string.Concat(currentIP!.AddressList.First().ToString(),':'),ipParts[1]);

builder.Services.AddHostedService<KafkaConsumer>(sp =>
{
    var producer = sp.GetRequiredService<Producer>();
    var consumer = sp.GetRequiredService<Consumer>();
    var config = new KafkaConfiguration(ctortopicPrimary: topicPrimary, ctorbrokerURL: stringIP, ctorgroupPrimary: groupPrimary, clientName: "AssetManager", ctorgroupSecondary: groupSecondary, ctortopicSecondary: topicSecondary);
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

/*app.UseHttpsRedirection();*/
app.UseStaticFiles(new StaticFileOptions
{
    RequestPath = "/images",
    FileProvider = new PhysicalFileProvider(path)
});
app.MapControllers();

app.Run();