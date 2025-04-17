
public interface IBackupRepository
{
    Task<byte[]> CreateDatabaseBackupAsync(string backupFolderPath);

    Task RestoreDatabaseBackupAsync(byte[] backupFileContent);
}
