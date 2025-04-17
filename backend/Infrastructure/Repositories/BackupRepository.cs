using System;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using backend.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class BackupRepository : IBackupRepository
{
  private readonly AppDbContext _context;

  public BackupRepository(AppDbContext context)
  {
    _context = context;
  }

  public async Task<byte[]> CreateDatabaseBackupAsync(string backupFolderPath)
  {
    if (!Directory.Exists(backupFolderPath))
      Directory.CreateDirectory(backupFolderPath);

    var dbName = _context.Database.GetDbConnection().Database;
    var backupFileName = $"{dbName}_{DateTime.Now:yyyyMMddHHmmss}.bak";
    var backupFullPath = Path.Combine(backupFolderPath, backupFileName);

    var sqlCommand = $"BACKUP DATABASE [{dbName}] TO DISK = '{backupFullPath}' WITH FORMAT";
    await _context.Database.ExecuteSqlRawAsync(sqlCommand);

    byte[] fileBytes = await File.ReadAllBytesAsync(backupFullPath);
    return fileBytes;
  }

  public async Task RestoreDatabaseBackupAsync(byte[] backupFileContent)
  {

    var tempPath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.bak");
    await File.WriteAllBytesAsync(tempPath, backupFileContent);

    var dbName = _context.Database.GetDbConnection().Database;
    var originalConnectionString = _context.Database.GetDbConnection().ConnectionString;

    await _context.Database.CloseConnectionAsync();
    await _context.DisposeAsync();

    var connectionStringBuilder = new SqlConnectionStringBuilder(originalConnectionString)
    {
      InitialCatalog = "master"
    };
    var masterConnectionString = connectionStringBuilder.ConnectionString;

    using var connection = new SqlConnection(masterConnectionString);
    await connection.OpenAsync();

    var killConnectionsSql = $@"
        DECLARE @killCommand VARCHAR(MAX) = '';
        SELECT @killCommand = @killCommand + 'KILL ' + CAST(session_id AS VARCHAR(5)) + '; '
        FROM sys.dm_exec_sessions
        WHERE database_id = DB_ID('{dbName}')
              AND session_id <> @@SPID;

        EXEC(@killCommand);
    ";
    using (var killCmd = connection.CreateCommand())
    {
      killCmd.CommandText = killConnectionsSql;
      await killCmd.ExecuteNonQueryAsync();
    }

    var restoreSql = $@"
        ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        RESTORE DATABASE [{dbName}] FROM DISK = '{tempPath}' WITH REPLACE;
        ALTER DATABASE [{dbName}] SET MULTI_USER;
    ";
    using (var restoreCmd = connection.CreateCommand())
    {
      restoreCmd.CommandText = restoreSql;
      await restoreCmd.ExecuteNonQueryAsync();
    }

    if (File.Exists(tempPath))
      File.Delete(tempPath);
  }


}
