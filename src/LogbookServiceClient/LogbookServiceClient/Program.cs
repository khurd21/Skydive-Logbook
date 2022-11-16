using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using LogbookServiceClient.Data;
using LogbookServiceClient.Models;
using LogbookService.Settings;
using Amazon.DynamoDBv2;
using LogbookService.Dependencies.DynamoDB;
using Amazon.DynamoDBv2.DataModel;
using LogbookService.Dependencies.LogbookService;
using Logbook.Dependencies.Mapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.Logging
    .ClearProviders()
    .AddConsole();


// Add services to the container.

// From LogbookService
builder.Services
    .AddSingleton<AmazonDynamoDBConfig>(
        provider => (AmazonDynamoDBConfig)new DynamoDBConfigProvider(provider.GetService<IConfiguration>()!).GetService(typeof(AmazonDynamoDBConfig))!)
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

// From LogbookServiceClient project
builder.Services
    .AddSingleton<AutoMapper.IConfigurationProvider>(
        provider =>(AutoMapper.IConfigurationProvider)new MapperConfigurationProvider().GetService(typeof(AutoMapper.IConfigurationProvider))!)
    .AddSingleton<AutoMapper.IMapper>(
        provider => (AutoMapper.IMapper)new AutoMapperProvider(provider.GetService<AutoMapper.IConfigurationProvider>()!).GetService(typeof(AutoMapper.IMapper))!);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(connectionString);
});
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddIdentityServerJwt()
    .AddGoogle(options =>
    {
        options.ClientId = ProjectSettings.GoogleClientId;
        options.ClientSecret = ProjectSettings.GoogleClientSecret;
    });

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    IDynamoDBTableManager tableManager = app.Services.GetService<IDynamoDBTableManager>()!;
    tableManager.DeleteTables();
    tableManager.CreateTables();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseIdentityServer();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapFallbackToFile("index.html");;

app.Run();
