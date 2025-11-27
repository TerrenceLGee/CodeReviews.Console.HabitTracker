using HabitTracker.TerrenceLGee.Data.Interfaces;
using HabitTracker.TerrenceLGee.HabitModels;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HabitTracker.TerrenceLGee.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(
        IConfiguration configuration,
        ILogger<UserRepository> logger)
    {
        _connectionString = configuration
                                .GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException(
                                "Unable to retrieve database connection string");
        _logger = logger;
    }

    public int InsertUser(User user)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"INSERT INTO users(FirstName, LastName) VALUES(@FirstName, @LastName);";

                sqliteCommand.Parameters
                    .AddWithValue("@FirstName", user.FirstName);
                sqliteCommand.Parameters
                    .AddWithValue("@LastName", user.LastName);

                return sqliteCommand.ExecuteNonQuery();
            }
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error adding the user to the database: " +
                    "{msg}\n", ex.Message);
            return -1;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "There was an unexpected error adding the user to the database: " +
                    "{msg}\n", ex.Message);
            return -1;
        }
    }

    public bool UserAlreadyExists(string firstName, string lastName)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"SELECT COUNT(*) FROM users WHERE FirstName LIKE @FirstName 
                    AND LastName LIKE @LastName;";

                sqliteCommand.Parameters
                    .AddWithValue("@FirstName", firstName);
                sqliteCommand.Parameters
                    .AddWithValue("@LastName", lastName);

                var count = (long)(sqliteCommand.ExecuteScalar() ?? 0);

                return count > 0;
            }
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error checking the existence of user " +
                    "{FirstName} {LastName} in the database: {msg}\n"
                , firstName, lastName, ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "There was an unexpected error checking the existence of user " +
                    "{FirstName} {LastName} in the database: {msg}\n"
                , firstName, lastName, ex.Message);
            return false;
        }
    }

    public User? GetUser(string firstName, string lastName)
    {
        User? user = null;
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"SELECT * FROM users WHERE FirstName LIKE @FirstName AND 
                           LastName LIKE @LastName;";

                sqliteCommand.Parameters
                    .AddWithValue("@FirstName", firstName);
                sqliteCommand.Parameters
                    .AddWithValue("@LastName", lastName);

                sqliteCommand.ExecuteNonQuery();
                

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new User
                        {
                            Id = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2)
                        };
                    }
                }
            }

            return user;
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error retrieving the user {FirstName} {LastName} from the database: {msg}\n"
                , firstName, lastName, ex.Message);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "There was an unexpected error retrieving the user {FirstName} {LastName} from the database: {msg}\n"
                , firstName, lastName, ex.Message);
            return user;
        }
    }

    
}