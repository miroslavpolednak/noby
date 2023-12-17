namespace DatabaseMigrations;

public enum ExitCode
{
    CodeAssemblyNotExist = -4,
    DirectoryNotExist = -3,
    MigrationFailed = -2,
    UnknownError = -1,
    NoMigrationAvailable = -1,
    Success = 0
}