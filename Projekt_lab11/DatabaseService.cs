using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;

namespace Projekt_lab11;

public class DatabaseService
{
    private readonly string _connectionString;

    public DatabaseService(string databasePath)
    {
        _connectionString = $"Data Source={databasePath}";
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Samples (
                Id TEXT PRIMARY KEY,
                Name TEXT NOT NULL,
                Type TEXT NOT NULL,
                DateCollected TEXT NOT NULL,
                Description TEXT
            );
        ";

        command.ExecuteNonQuery();
    }

    public List<Sample> LoadSamples()
    {
        var samples = new List<Sample>();

        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT Id, Name, Type, DateCollected, Description
            FROM Samples
            ORDER BY DateCollected DESC, Name ASC;
        ";

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            samples.Add(new Sample
            {
                Id = reader.GetString(0),
                Name = reader.GetString(1),
                Type = reader.GetString(2),
                DateCollected = DateTime.Parse(reader.GetString(3)),
                Description = reader.IsDBNull(4)
                    ? string.Empty
                    : reader.GetString(4)
            });
        }

        return samples;
    }

    public bool SampleExists(string id)
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT COUNT(1) FROM Samples WHERE Id = $id";

        command.Parameters.AddWithValue("$id", id);

        return Convert.ToInt32(command.ExecuteScalar() ?? 0) > 0;
    }

    public bool SampleExists(string id, string excludeId)
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            @"SELECT COUNT(1)
              FROM Samples
              WHERE Id = $id
              AND Id <> $excludeId";

        command.Parameters.AddWithValue("$id", id);
        command.Parameters.AddWithValue("$excludeId", excludeId);

        return Convert.ToInt32(command.ExecuteScalar() ?? 0) > 0;
    }

    public void AddSample(Sample sample)
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = @"
            INSERT INTO Samples
            (Id, Name, Type, DateCollected, Description)

            VALUES
            ($id, $name, $type, $dateCollected, $description);
        ";

        command.Parameters.AddWithValue("$id", sample.Id);
        command.Parameters.AddWithValue("$name", sample.Name);
        command.Parameters.AddWithValue("$type", sample.Type);

        command.Parameters.AddWithValue(
            "$dateCollected",
            sample.DateCollected.ToString("yyyy-MM-dd"));

        command.Parameters.AddWithValue(
            "$description",
            sample.Description);

        command.ExecuteNonQuery();
    }

    public void UpdateSample(string originalId, Sample sample)
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText = @"
            UPDATE Samples

            SET
                Id = $id,
                Name = $name,
                Type = $type,
                DateCollected = $dateCollected,
                Description = $description

            WHERE Id = $originalId;
        ";

        command.Parameters.AddWithValue("$id", sample.Id);
        command.Parameters.AddWithValue("$name", sample.Name);
        command.Parameters.AddWithValue("$type", sample.Type);

        command.Parameters.AddWithValue(
            "$dateCollected",
            sample.DateCollected.ToString("yyyy-MM-dd"));

        command.Parameters.AddWithValue(
            "$description",
            sample.Description);

        command.Parameters.AddWithValue(
            "$originalId",
            originalId);

        command.ExecuteNonQuery();
    }

    public void DeleteSample(string id)
    {
        using var connection = new SqliteConnection(_connectionString);

        connection.Open();

        using var command = connection.CreateCommand();

        command.CommandText =
            @"DELETE FROM Samples WHERE Id = $id";

        command.Parameters.AddWithValue("$id", id);

        command.ExecuteNonQuery();
    }
}