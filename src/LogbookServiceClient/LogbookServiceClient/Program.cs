using Amazon.DynamoDBv2;
using LogbookService.Dependencies.DynamoDB;
using LogbookService.Dependencies.LogbookService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddSingleton<AmazonDynamoDBConfig>(
        provider =>(AmazonDynamoDBConfig)new DynamoDBConfigProvider(provider.GetService<IConfiguration>()!).GetService(typeof(AmazonDynamoDBConfig))!)
    .AddSingleton<AmazonDynamoDBClient>(
        provider => (AmazonDynamoDBClient)new DynamoDBClientProvider(provider.GetService<AmazonDynamoDBConfig>()!).GetService(typeof(AmazonDynamoDBClient))!)
    .AddSingleton<ILogbookService, LogbookServiceProvider>()
    .AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Initialize the Database Tables
    AmazonDynamoDBClient client = app.Services.GetService<AmazonDynamoDBClient>()!;
    ILogger logger = app.Services.GetService<ILogger>()!;
    DynamoDBTableManager tableManager = new(client, logger);
    tableManager.ReinitializeTables();

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
