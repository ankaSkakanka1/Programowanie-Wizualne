using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

public class Database
{
    private string connectionString = "Data Source=database.db;";

    public void Init()
    {
        try
        {
            TryInit();
        }
        catch (SqliteException ex) when (ex.SqliteErrorCode == 26)
        {
            if (File.Exists("database.db"))
            {
                File.Delete("database.db");
            }

            TryInit();
        }
    }

    private void TryInit()
    {
        using var conn = new SqliteConnection(connectionString);
        conn.Open();

        string dropQuery = "DROP TABLE IF EXISTS Wnioski";
        using var dropCmd = new SqliteCommand(dropQuery, conn);
        dropCmd.ExecuteNonQuery();

        string query = @"CREATE TABLE Wnioski (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            ImieNazwisko TEXT,
            NumerAlbumu TEXT,
            Kierunek TEXT,
            Specjalnosc TEXT,
            RokStudiow TEXT,
            FormaStudiow TEXT,
            Przedmiot TEXT,
            Prowadzacy TEXT,
            DataEgzaminu TEXT,
            Ocena TEXT,
            Uzasadnienie TEXT,
            DataWniosku TEXT
        )";

        using var cmd = new SqliteCommand(query, conn);
        cmd.ExecuteNonQuery();
    }

    public void AddWniosek(string imie, string album, string kierunek, string specjalnosc,
        string rokStudiow, string formaStudiow, string przedmiot, string prowadzacy,
        string dataEgzaminu, string ocena, string uzasadnienie, string dataWniosku)
    {
        Console.WriteLine("Próba zapisu do bazy...");
        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        Console.WriteLine("Połączenie otwarte.");

        string query = @"INSERT INTO Wnioski
        (ImieNazwisko, NumerAlbumu, Kierunek, Specjalnosc, RokStudiow, FormaStudiow, Przedmiot, Prowadzacy, DataEgzaminu, Ocena, Uzasadnienie, DataWniosku)
        VALUES (@i, @a, @k, @s, @r, @f, @p, @pr, @d, @o, @u, @dw)";

        using var cmd = new SqliteCommand(query, conn);
        cmd.Parameters.AddWithValue("@i", imie);
        cmd.Parameters.AddWithValue("@a", album);
        cmd.Parameters.AddWithValue("@k", kierunek);
        cmd.Parameters.AddWithValue("@s", specjalnosc);
        cmd.Parameters.AddWithValue("@r", rokStudiow);
        cmd.Parameters.AddWithValue("@f", formaStudiow);
        cmd.Parameters.AddWithValue("@p", przedmiot);
        cmd.Parameters.AddWithValue("@pr", prowadzacy);
        cmd.Parameters.AddWithValue("@d", dataEgzaminu);
        cmd.Parameters.AddWithValue("@o", ocena);
        cmd.Parameters.AddWithValue("@u", uzasadnienie);
        cmd.Parameters.AddWithValue("@dw", dataWniosku);

        cmd.ExecuteNonQuery();
        Console.WriteLine("Zapis zakończony.");
    }

    public List<string> ReadAll()
    {
        Console.WriteLine("Próba odczytu z bazy...");
        var results = new List<string>();

        using var conn = new SqliteConnection(connectionString);
        conn.Open();
        Console.WriteLine("Połączenie otwarte dla odczytu.");

        string query = "SELECT * FROM Wnioski ORDER BY Id DESC";

        using var cmd = new SqliteCommand(query, conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            results.Add($"{reader["ImieNazwisko"]} | {reader["NumerAlbumu"]} | {reader["Kierunek"]} | {reader["Specjalnosc"]} | {reader["RokStudiow"]} | {reader["FormaStudiow"]} | {reader["Przedmiot"]} | {reader["Prowadzacy"]} | {reader["DataEgzaminu"]} | {reader["Ocena"]} | {reader["Uzasadnienie"]} | {reader["DataWniosku"]}");
        }
        Console.WriteLine($"Odczyt zakończony, {results.Count} wniosków.");
        return results;
    }
}