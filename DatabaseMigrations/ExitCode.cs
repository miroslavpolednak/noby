namespace DatabaseMigrations;

public enum ExitCode
{
    NoMigrationAvailable = -1,
    Success = 0,
    UnknownError,
    CodeAssemblyNotExist,
    DirectoryNotExist,
    MigrationFailed
}