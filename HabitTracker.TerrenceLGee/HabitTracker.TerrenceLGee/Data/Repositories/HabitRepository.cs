using System.Globalization;
using HabitTracker.TerrenceLGee.Data.Interfaces;
using HabitTracker.TerrenceLGee.Extensions;
using HabitTracker.TerrenceLGee.HabitModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace HabitTracker.TerrenceLGee.Data.Repositories;

public class HabitRepository : IHabitRepository
{
    private readonly string _connectionString;
    private readonly ILogger<HabitRepository> _logger;

    public HabitRepository(
        IConfiguration configuration,
        ILogger<HabitRepository> logger)
    {
        _connectionString = configuration
                                .GetConnectionString("DefaultConnection")
                            ?? throw new InvalidOperationException(
                                "Unable to retrieve database connection string");
        _logger = logger;
    }

    public int Insert(Habit habit)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"INSERT INTO habits(UserId, Name, DateOfOccurrence, UnitOfMeasurement, 
                   Quantity, Comments) 
                    VALUES(@UserId, @Name, @DateOfOccurrence, @UnitOfMeasurement,@Quantity, @Comments);";

                sqliteCommand.Parameters
                    .AddWithValue("@UserId", habit.UserId);
                sqliteCommand.Parameters
                    .AddWithValue("@Name", habit.Name);
                sqliteCommand.Parameters
                    .AddWithValue("@DateOfOccurrence", habit.DateOfOccurrence);
                sqliteCommand.Parameters
                    .AddWithValue("@UnitOfMeasurement", habit.UnitOfMeasurement.GetDisplayName());
                sqliteCommand.Parameters
                    .AddWithValue("@Quantity", habit.Quantity);
                sqliteCommand.Parameters
                    .AddWithValue("@Comments", habit.Comments ?? (object)DBNull.Value);

                return sqliteCommand.ExecuteNonQuery();
            }
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error inserting the habit into the database: " +
                    "{msg}\n", ex.Message);
            return -1;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "An unexpected error occurred while inserting the habit: " +
                    "{msg}\n", ex.Message);
            return -1;
        }
    }

    public int Update(Habit habit)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"UPDATE habits SET Name = @Name, DateOfOccurrence = @DateOfOccurrence, 
                  UnitOfMeasurement = @UnitOfMeasurement, Quantity = @Quantity, Comments = @Comments 
                  WHERE Id = @Id and UserId = @UserId;";

                sqliteCommand.Parameters
                    .AddWithValue("@Name", habit.Name);
                sqliteCommand.Parameters
                    .AddWithValue("@DateOfOccurrence", habit.DateOfOccurrence);
                sqliteCommand.Parameters
                    .AddWithValue("@UnitOfMeasurement", habit.UnitOfMeasurement.GetDisplayName());
                sqliteCommand.Parameters
                    .AddWithValue("@Quantity", habit.Quantity);
                sqliteCommand.Parameters
                    .AddWithValue("@Comments", habit.Comments ?? (object)DBNull.Value);
                sqliteCommand.Parameters
                    .AddWithValue("@Id", habit.Id);
                sqliteCommand.Parameters
                    .AddWithValue("@UserId", habit.UserId);

                return sqliteCommand.ExecuteNonQuery();
            }
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error updating the habit in the database with id {id}:" +
                    " {msg}\n", habit.Id,  ex.Message);
            return -1;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "An unexpected error occurred while updating the habit with id: {id}: " +
                    "{msg}\n", habit.Id, ex.Message);
            return -1;
        }
    }

    public int Delete(int userId, int habitId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"DELETE FROM habits WHERE Id = @Id AND UserId = @UserId;";

                sqliteCommand.Parameters
                    .AddWithValue("@Id", habitId);
                sqliteCommand.Parameters
                    .AddWithValue("@UserId", userId);

                return sqliteCommand.ExecuteNonQuery();
            }
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error deleting the habit from the database with id {id}: " +
                    "{msg}\n", habitId,  ex.Message);
            return -1;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "An unexpected error occurred while deleting the habit with id: {id}: " +
                    "{msg}\n", habitId, ex.Message);
            return -1;
        }
    }

    public bool HabitExists(int userId, int habitId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"SELECT COUNT(*) FROM habits WHERE Id = @Id AND UserId = @UserId;";

                sqliteCommand.Parameters
                    .AddWithValue("@Id", habitId);
                sqliteCommand.Parameters
                    .AddWithValue("@UserId", userId);

                var count = (long)(sqliteCommand.ExecuteScalar() ?? 0);

                return count == 1;
            }
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error checking the existence of habit {habitId} " +
                    "for user {userId} in the database: {msg}\n"
                , habitId, userId, ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "There was an unexpected error checking the existence of habit {habitId} " +
                    "for user {userId} in the database: {msg}\n"
                ,habitId, userId, ex.Message);
            return false;
        }
    }

    public bool AnyHabitsExistForUser(int userId)
    {
        try
        {
            using (var connection = new SqliteConnection(_connectionString)) 
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"SELECT COUNT(*) FROM habits WHERE UserId = @UserId;";

                sqliteCommand.Parameters
                    .AddWithValue("@UserId", userId);

                var count = (long)(sqliteCommand.ExecuteScalar() ?? 0);

                return count > 0;
            }
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error checking the existence of any habits " +
                    "for user {userId} in the database: {msg}\n"
                , userId, ex.Message);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "There was an unexpected error checking the existence of any habits  " +
                    "for user {userId} in the database: {msg}\n"
                , userId,  ex.Message);
            return false;
        }
    }

    public Habit? GetHabitForUser(int userId, int habitId)
    {
        Habit? habit = null;
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"SELECT * FROM habits WHERE UserId = @UserId AND Id = @Id;";

                sqliteCommand.Parameters
                    .AddWithValue("@UserId", userId);
                sqliteCommand.Parameters
                    .AddWithValue("@Id", habitId);

                sqliteCommand.ExecuteNonQuery();

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        habit = new Habit
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            UserId = reader.GetInt32(2),
                            DateOfOccurrence = DateOnly.ParseExact(
                                reader.GetString(3),
                                "yyyy-M-dd",
                                CultureInfo.InvariantCulture),
                            UnitOfMeasurement = Enum.Parse<UnitOfMeasurement>(reader.GetString(4)),
                            Quantity = reader.GetInt32(5),
                            Comments = reader.IsDBNull(6)
                            ? null
                            : reader.GetString(6)
                        };
                    }
                }
            }

            return habit;
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error retrieving the habit from the database with id {id}" +
                    " for user {user}: {msg}\n", 
                habitId, userId,  ex.Message);
            return habit;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "An unexpected error occurred while retrieving the habit with id {id} " +
                    "for user {user}: {msg}\n",
                habitId, userId, ex.Message);
            return habit;
        }
    }

    public List<Habit> GetHabitsForUser(int userId)
    {
        List<Habit> habits = [];
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"SELECT * FROM habits WHERE UserId = @UserId;";

                sqliteCommand.Parameters
                    .AddWithValue("@UserId", userId);

                sqliteCommand.ExecuteNonQuery();
                GetHabitsFromDatabase(sqliteCommand, habits);
            }

            return habits;
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error retrieving the habits from the database for user {user}:" +
                    " {msg}\n", userId,  ex.Message);
            return habits;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "An unexpected error occurred while retrieving the habits for user {user}: " +
                    "{msg}\n", userId, ex.Message);
            return habits;
        }
    }
    
    public List<string> GetHabitNamesForUser(int userId)
    {
        List<string> habitNames = [];
        try
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                var sqliteCommand = connection.CreateCommand();

                sqliteCommand.CommandText =
                    @"SELECT DISTINCT Name FROM habits WHERE 
                                     UserId = @UserId ORDER BY Name;";

                sqliteCommand.Parameters
                    .AddWithValue("@UserId", userId);

                sqliteCommand.ExecuteNonQuery();

                using (var reader = sqliteCommand.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            habitNames.Add(reader.GetString(0));
                        }
                    }
                }
            }

            return habitNames;
        }
        catch (SqliteException ex)
        {
            _logger.LogError(
                ex, "There was an error retrieving the habit names from the database for user " +
                    "{user}: {msg}\n", userId, ex.Message);
            return habitNames;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "An unexpected error occurred while retrieving the habit names for user " +
                    "{user}: {msg}\n", userId, ex.Message);
            return habitNames;
        }
    }

    private void GetHabitsFromDatabase(SqliteCommand sqliteCommand, List<Habit> habits)
    {
        using (var reader = sqliteCommand.ExecuteReader())
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    habits.Add(
                        new Habit
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            UserId = reader.GetInt32(2),
                            DateOfOccurrence = DateOnly.ParseExact(
                                reader.GetString(3),
                                "yyyy-M-d",
                                CultureInfo.InvariantCulture),
                            UnitOfMeasurement = Enum.Parse<UnitOfMeasurement>(reader.GetString(4)),
                            Quantity = reader.GetInt32(5),
                            Comments =  reader.IsDBNull(6)
                                ? null 
                                : reader.GetString(6),
                        });
                }
            }
        }
    }
}