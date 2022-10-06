using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using LogbookService.Dependencies.DynamoDB;
using LogbookService.Dependencies.LogbookService;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging
    .ClearProviders()
    .AddConsole();

// Add services to the container.
builder.Services
    .AddSingleton<AmazonDynamoDBConfig>(
        provider =>(AmazonDynamoDBConfig)new DynamoDBConfigProvider(provider.GetService<IConfiguration>()!).GetService(typeof(AmazonDynamoDBConfig))!)
    .AddSingleton<AmazonDynamoDBClient>(
        provider => (AmazonDynamoDBClient)new DynamoDBClientProvider(provider.GetService<AmazonDynamoDBConfig>()!).GetService(typeof(AmazonDynamoDBClient))!)
    .AddSingleton<DynamoDBContextConfig>(
        provider => (DynamoDBContextConfig)new DynamoDBContextConfigProvider().GetService(typeof(DynamoDBContextConfig))!)
    .AddSingleton<DynamoDBContext>(
        provider => (DynamoDBContext)new DynamoDBContextProvider(provider.GetService<AmazonDynamoDBClient>()!, provider.GetService<DynamoDBContextConfig>()!).GetService(typeof(DynamoDBContext))!)
    .AddSingleton<ILogbookService, LogbookServiceProvider>()
    .AddSingleton<IDynamoDBTableManager, DynamoDBTableManager>()
    .AddSingleton<IConfiguration>(builder.Configuration)
    .AddSingleton<ILogger>(provider => provider.GetService<ILoggerFactory>()!.CreateLogger("LogbookServiceClient"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Initialize the Database Tables
    IDynamoDBTableManager tableManager = app.Services.GetService<IDynamoDBTableManager>()!;
    // tableManager.DeleteTables();
    // tableManager.CreateTables();

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
