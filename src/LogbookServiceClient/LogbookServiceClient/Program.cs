using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Logbook.Authorization;
using LogbookService.Dependencies.AuthenticationService;
using LogbookService.Dependencies.DynamoDB;
using LogbookService.Dependencies.LogbookService;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Identity;

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
        provider => (DynamoDBContextConfig)new DynamoDBContextConfigProvider(provider.GetService<IConfiguration>()!).GetService(typeof(DynamoDBContextConfig))!)
    .AddSingleton<IDynamoDBContext>(
        provider => (DynamoDBContext)new DynamoDBContextProvider(provider.GetService<AmazonDynamoDBClient>()!, provider.GetService<DynamoDBContextConfig>()!).GetService(typeof(DynamoDBContext))!)
    .AddSingleton<ILogbookService, LogbookServiceProvider>()
    //
    .AddSingleton<IAuthenticationService, AuthenticationServiceProvider>()
    .AddScoped<IJwtUtils, JwtUtils>()
    //
    .AddSingleton<IDynamoDBTableManager, DynamoDBTableManager>()
    .AddSingleton<IConfiguration>(builder.Configuration)
    .AddSingleton<ILogger>(provider => provider.GetService<ILoggerFactory>()!.CreateLogger("LogbookServiceClient"));

builder.Services.AddCors();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Initialize the Database Tables
    IDynamoDBTableManager tableManager = app.Services.GetService<IDynamoDBTableManager>()!;
    tableManager.DeleteTables();
    tableManager.CreateTables();

    app.UseDeveloperExceptionPage();
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseAuthentication();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseMiddleware<ErrorHandlerMiddleware>();
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
