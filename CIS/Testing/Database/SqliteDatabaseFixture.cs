using Microsoft.Data.Sqlite;

namespace CIS.Testing.Database;

public class SqliteDatabaseFixture : IDatabaseFixture
{
    public Core.Data.IConnectionProvider Provider { get; init; }

    private readonly string _databasePath;
    private readonly DatabaseFixtureOptions _options;

    public SqliteDatabaseFixture(DatabaseFixtureOptions options)
    {
        _options = options;

        // pridat Sqlite connection string provider
        _databasePath = Path.Combine(getCurrentDir(), Guid.NewGuid().ToString() + ".db");
        Provider = new SqliteConnectionProvider($"Data Source={_databasePath};");

        if (!string.IsNullOrEmpty(_options.SeedPaths))
            Seed();
    }

    public void Dispose()
    {
        if (File.Exists(_databasePath))
            File.Delete(_databasePath);
    }

    private void Seed()
    {
        var seeds = _options.SeedPaths?.Split(";", StringSplitOptions.RemoveEmptyEntries) ?? new string[0];
        foreach (var seed in seeds)
        {
            string path;
            // je zadana relativni cesta - pak nahrad ~ aktualnim adresarem + adresarem s testama
            if (seed.StartsWith("~"))
                path = Path.Combine(getCurrentDir(), GlobalTestsSettings.TestsFolderName, seed.Substring(2));
            else
                path = seed;

            if (File.Exists(path))
            {
                string script = File.ReadAllText(path);

                using (var connection = (SqliteConnection)Provider.Create())
                {
                    connection.Open();
                    using (var cmd = new SqliteCommand(script, connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    private string getCurrentDir()
        => Directory.GetCurrentDirectory();
}
