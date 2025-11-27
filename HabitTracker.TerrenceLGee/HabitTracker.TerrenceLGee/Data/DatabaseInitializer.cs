using HabitTracker.TerrenceLGee.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace HabitTracker.TerrenceLGee.Data;

public class DatabaseInitializer : IDatabaseInitializer
{
    private readonly string _connectionString;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(
        IConfiguration configuration,
        ILogger<DatabaseInitializer> logger)
    {
        _connectionString = configuration
                                .GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException(
                                "Unable to retrieve database connection string");
        _logger = logger;
    }

    public void InitializeDatabase()
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var sqliteCommand = connection.CreateCommand())
                {
                    sqliteCommand.CommandText =
                        @"CREATE TABLE IF NOT EXISTS users (
                          Id INTEGER PRIMARY KEY AUTOINCREMENT,
                          FirstName TEXT,
                          LastName TEXT);";

                    sqliteCommand.ExecuteNonQuery();

                    sqliteCommand.CommandText =
                        @"CREATE TABLE IF NOT EXISTS habits(
                          Id INTEGER PRIMARY KEY AUTOINCREMENT,
                          Name TEXT,
                          UserId INTEGER,
                          DateOfOccurrence TEXT,
                          UnitOfMeasurement TEXT,
                          Quantity INTEGER,
                          Comments TEXT,
                          FOREIGN KEY(UserId) References users(id)
                          ON DELETE CASCADE);";

                    sqliteCommand.ExecuteNonQuery();
                }
            }
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "An error occurred initializing the database: {msg}", ex.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred: {msg}", ex.Message);
            throw;
        }
    }
}