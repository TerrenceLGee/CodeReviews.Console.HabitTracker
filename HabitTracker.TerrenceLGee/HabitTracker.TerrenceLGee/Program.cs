using HabitTracker.TerrenceLGee.Data;
using HabitTracker.TerrenceLGee.Data.Interfaces;
using HabitTracker.TerrenceLGee.Data.Repositories;
using HabitTracker.TerrenceLGee.HabitTrackerUI;
using HabitTracker.TerrenceLGee.HabitTrackerUI.Interfaces;
using HabitTracker.TerrenceLGee.Services;
using HabitTracker.TerrenceLGee.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;
using Microsoft.Extensions.DependencyInjection;

try
{
    LoggingSetup();
    Startup();
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected error occurred starting this program: {ex.Message}");
}

void Startup()
{
    IConfiguration configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();

    var services = new ServiceCollection()
        .AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true))
        .AddSingleton(configuration)
        .AddSingleton<ITrackerOperationsUi, TrackerOperationsUi>()
        .AddScoped<IDatabaseInitializer, DatabaseInitializer>()
        .AddScoped<IUserRepository, UserRepository>()
        .AddScoped<IHabitRepository, HabitRepository>()
        .AddScoped<IUserService, UserService>()
        .AddScoped<IHabitService, HabitService>();

    var serviceProvider = services.BuildServiceProvider();

    var database = serviceProvider.GetRequiredService<IDatabaseInitializer>();

    database.InitializeDatabase();

    var trackerUi = serviceProvider.GetRequiredService<ITrackerOperationsUi>();

    var app = new HabitTrackerApp(trackerUi);
    app.Run();
}

void LoggingSetup()
{
    var loggingDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
    Directory.CreateDirectory(loggingDirectory);
    var filePath = Path.Combine(loggingDirectory, "app-.txt");
    var outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File(
            path: filePath,
            rollingInterval: RollingInterval.Day,
            outputTemplate: outputTemplate)
        .CreateLogger();
}