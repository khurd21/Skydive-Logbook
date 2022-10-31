using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Logbook.Dependencies.Mapper;
using LogbookService.Dependencies.DynamoDB;
using LogbookService.Dependencies.LogbookService;
using LogbookService.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;

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
    .AddSingleton<IDynamoDBTableManager, DynamoDBTableManager>()
    .AddSingleton<IConfiguration>(builder.Configuration)
    .AddSingleton<ILogger>(provider => provider.GetService<ILoggerFactory>()!.CreateLogger("LogbookServiceClient"));


// Services from LogbookServiceClient
builder.Services
    .AddSingleton<AutoMapper.IConfigurationProvider>(
        provider =>(AutoMapper.IConfigurationProvider)new MapperConfigurationProvider().GetService(typeof(AutoMapper.IConfigurationProvider))!)
    .AddSingleton<AutoMapper.IMapper>(
        provider => (AutoMapper.IMapper)new AutoMapperProvider(provider.GetService<AutoMapper.IConfigurationProvider>()!).GetService(typeof(AutoMapper.IMapper))!);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddNegotiate()
    .AddGoogle(options =>
    {
        options.ClientId = ProjectSettings.GoogleClientId;
        options.ClientSecret = ProjectSettings.GoogleClientSecret;
        options.SaveTokens = true;
        /*options.Events.OnRedirectToAuthorizationEndpoint = context =>
        {
            context.Response.Redirect(context.RedirectUri + "&prompt=consent");
            return Task.CompletedTask;
        };*/
    });


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Initialize the Database Tables
    IDynamoDBTableManager tableManager = app.Services.GetService<IDynamoDBTableManager>()!;
    tableManager.DeleteTables();
    tableManager.CreateTables();

    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
