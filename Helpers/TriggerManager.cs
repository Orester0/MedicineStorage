using MedicineStorage.Data.Interfaces;

namespace MedicineStorage.Helpers
{
    public class TriggerManager(IUnitOfWork _unitOfWork) : ITriggerManager
    {
        public async Task DisableTriggersAsync()
        {
            string sql = @"
        DECLARE @sql NVARCHAR(MAX) = N'';

        SELECT @sql += 'DISABLE TRIGGER [' + t.name + '] ON [' + s.name + '].[' + o.name + '];' + CHAR(13)
        FROM sys.triggers t
        JOIN sys.objects o ON t.parent_id = o.object_id
        JOIN sys.schemas s ON o.schema_id = s.schema_id
        WHERE t.is_ms_shipped = 0;

        EXEC sp_executesql @sql;
    ";
            await _unitOfWork.ExecuteSqlRawAsync(sql);
        }

        public async Task EnableTriggersAsync()
        {
            string sql = @"
        DECLARE @sql NVARCHAR(MAX) = N'';

        SELECT @sql += 'ENABLE TRIGGER [' + t.name + '] ON [' + s.name + '].[' + o.name + '];' + CHAR(13)
        FROM sys.triggers t
        JOIN sys.objects o ON t.parent_id = o.object_id
        JOIN sys.schemas s ON o.schema_id = s.schema_id
        WHERE t.is_ms_shipped = 0;

        EXEC sp_executesql @sql;
    ";
            await _unitOfWork.ExecuteSqlRawAsync(sql);
        }

    }
}
