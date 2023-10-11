namespace DatabaseMigrations;

public enum ExitCode
{
    DirectoryNotExist = -3,
    MigrationFailed = -2,
    UnknownError = -1,
    Success = 0,
    NoMigrationAvailable = 1
}