
using MedicineStorage.Data.Interfaces;


namespace MedicineStorage.Helpers
{
    public class StoredProceduresGenerator(IUnitOfWork _unitOfWork) : IStoredProceduresGenerator
    {
        public async Task CreateCleanupUnusedCategoryTriggerAsync()
        {
            string script = @"
                IF OBJECT_ID('TRG_CleanupUnusedCategory', 'TR') IS NOT NULL
                    DROP TRIGGER TRG_CleanupUnusedCategory;

                EXEC('
                    CREATE TRIGGER TRG_CleanupUnusedCategory
                    ON Medicines
                    AFTER DELETE, UPDATE
                    AS
                    BEGIN
                        SET NOCOUNT ON;

                        ;WITH AffectedCategories AS (
                            SELECT DISTINCT CategoryId
                            FROM deleted
                            WHERE CategoryId IS NOT NULL
                        )

                        DELETE FROM MedicineCategories
                        WHERE Id IN (
                            SELECT ac.CategoryId
                            FROM AffectedCategories ac
                            LEFT JOIN Medicines m ON ac.CategoryId = m.CategoryId
                            WHERE m.Id IS NULL
                        );
                    END
                ');
            "
            ;

            await _unitOfWork.ExecuteSqlRawAsync(script);
        }

        //SELECT* FROM MEDICINES WHERE NAME = 'ZXC'

        //SELECT* FROM MedicineCategories WHERE ID = 110

        //DELETE FROM MEDICINES WHERE NAME = 'ZXC'

        //SELECT* FROM MedicineCategories WHERE ID = 110


        public async Task CreateGetOrInsertCategoryProcedureAsync()
        {
            string script = @"
        IF OBJECT_ID('sp_GetOrInsertCategory', 'P') IS NOT NULL
            DROP PROCEDURE sp_GetOrInsertCategory;
        
        EXEC('
            CREATE PROCEDURE sp_GetOrInsertCategory
                @Name NVARCHAR(255),
                @Id INT OUTPUT
            AS
            BEGIN
                SET NOCOUNT ON;

                SELECT @Id = Id
                FROM MedicineCategories
                WHERE LOWER(Name) = LOWER(@Name);

                IF @Id IS NULL
                BEGIN
                    INSERT INTO MedicineCategories (Name)
                    VALUES (@Name);

                    SET @Id = SCOPE_IDENTITY();
                END
            END
            ');
        ";

            await _unitOfWork.ExecuteSqlRawAsync(script);
        }

        //DECLARE @Id INT;

        //EXEC sp_GetOrInsertCategory
        //    @Name = N'Pain-Relief',  
        //    @Id = @Id OUTPUT;

        //SELECT @Id AS CategoryId;


        public async Task CreateUpdateMinimumStockProcedureAsync()
        {
            string script = @"
                IF OBJECT_ID('sp_UpdateMinimumStockForecast', 'P') IS NOT NULL
                    DROP PROCEDURE sp_UpdateMinimumStockForecast;

                EXEC('
                CREATE PROCEDURE sp_UpdateMinimumStockForecast
                    @ForecastDays INT = 30
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @StartDate DATETIME = DATEADD(MONTH, -3, GETUTCDATE());
                    DECLARE @EndDate DATETIME = GETUTCDATE();
                    DECLARE @DaysInPeriod FLOAT = DATEDIFF(DAY, @StartDate, @EndDate);
                    DECLARE @SafetyBuffer FLOAT = 1.5;
                    DECLARE @AdjustmentRate FLOAT = 0.1; -- 10% корекція

                    CREATE TABLE #MedicineStockUpdates (
                        MedicineId INT PRIMARY KEY,
                        NewMinimumStock INT
                    );

                    ;WITH RequestSums AS (
                        SELECT 
                            r.MedicineId,
                            SUM(r.Quantity) AS TotalApprovedRequestQty
                        FROM MedicineRequests r
                        WHERE r.Status = 1
                          AND r.RequestDate BETWEEN @StartDate AND @EndDate
                        GROUP BY r.MedicineId
                    ),
                    SupplySums AS (
                        SELECT 
                            s.MedicineId,
                            SUM(s.Quantity) AS TotalSupplyQty
                        FROM MedicineSupplies s
                        WHERE s.TransactionDate BETWEEN @StartDate AND @EndDate
                        GROUP BY s.MedicineId
                    )

                    INSERT INTO #MedicineStockUpdates (MedicineId, NewMinimumStock)
                    SELECT 
                        m.Id AS MedicineId,
                        CASE
                            WHEN @DaysInPeriod > 0 THEN 
                                CASE 
                                    WHEN CEILING(
                                            (CAST(
                                                ISNULL(r.TotalApprovedRequestQty, 0) - ISNULL(s.TotalSupplyQty, 0)
                                                AS FLOAT
                                            ) / @DaysInPeriod) * @ForecastDays * @SafetyBuffer
                                         ) < 1 
                                    THEN 1
                                    ELSE CEILING(
                                            (CAST(
                                                ISNULL(r.TotalApprovedRequestQty, 0) - ISNULL(s.TotalSupplyQty, 0)
                                                AS FLOAT
                                            ) / @DaysInPeriod) * @ForecastDays * @SafetyBuffer
                                         )
                                END
                            ELSE 1
                        END AS NewMinimumStock
                    FROM Medicines m
                    LEFT JOIN RequestSums r ON m.Id = r.MedicineId
                    LEFT JOIN SupplySums s ON m.Id = s.MedicineId;

                    UPDATE m
                    SET m.MinimumStock = 
                        CASE 
                            WHEN u.NewMinimumStock > m.MinimumStock * (1 + @AdjustmentRate) THEN CEILING(m.MinimumStock * (1 + @AdjustmentRate))
                            WHEN u.NewMinimumStock < m.MinimumStock * (1 - @AdjustmentRate) THEN FLOOR(m.MinimumStock * (1 - @AdjustmentRate))
                            ELSE u.NewMinimumStock
                        END
                    FROM Medicines m
                    INNER JOIN #MedicineStockUpdates u ON m.Id = u.MedicineId;

                    DROP TABLE #MedicineStockUpdates;
                END
                ')
                ";

            await _unitOfWork.ExecuteSqlRawAsync(script);
        }

        //        SELECT TOP 50 
        //    Id, 
        //    Name, 
        //    MinimumStock AS CurrentMinimumStock
        //FROM
        //    Medicines
        //ORDER BY
        //    Name;

        //        EXEC sp_UpdateMinimumStockForecast @ForecastDays = 30;

        //SELECT TOP 50 
        //    Id, 
        //    Name, 
        //    MinimumStock AS UpdatedMinimumStock
        //FROM
        //    Medicines
        //ORDER BY
        //Name;



        public async Task CreateCheckMedicineRequestApprovalTriggerAsync()
        {
            string script = @"
        IF OBJECT_ID('TRG_CheckMedicineRequestApproval', 'TR') IS NOT NULL
            DROP TRIGGER TRG_CheckMedicineRequestApproval;
        EXEC('
            CREATE TRIGGER [dbo].[TRG_CheckMedicineRequestApproval]
            ON [dbo].[MedicineRequests]
            AFTER UPDATE
            AS
            BEGIN
                SET NOCOUNT ON;

                IF EXISTS (
                    SELECT 1
                    FROM inserted I
                    INNER JOIN deleted D ON I.Id = D.Id
                    WHERE I.Status = 3 AND D.Status = 2
                )
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM inserted I
                        LEFT JOIN [dbo].[AspNetUserRoles] UR ON I.ApprovedByUserId = UR.UserId
                        LEFT JOIN [dbo].[AspNetRoles] R ON UR.RoleId = R.Id
                        WHERE R.Name IS NULL OR R.Name NOT IN (''Admin'', ''Manager'')
                    )
                    BEGIN
                        ROLLBACK TRANSACTION;
                        RAISERROR(''Only Admin or Manager can reject an approved request.'', 16, 1);
                        RETURN;
                    END
                END
            END
        ');
    ";
            await _unitOfWork.ExecuteSqlRawAsync(script);
        }
//        BEGIN TRY
//    BEGIN TRANSACTION;

//        DECLARE @MedicineId INT;
//    SELECT TOP 1 @MedicineId = M.Id
//    FROM dbo.Medicines M
//    WHERE M.RequiresSpecialApproval = 1
//      AND EXISTS (
//          SELECT 1
//          FROM dbo.MedicineRequests MR
//          WHERE MR.MedicineId = M.Id AND MR.Status = 2
//      );

//        IF @MedicineId IS NULL
//        RAISERROR('❌ Не знайдено ліків, які потребують дозволу та мають запити зі статусом 2.', 16, 1);

//        DECLARE @UserId NVARCHAR(450);
//        SELECT TOP 1 @UserId = U.Id
//        FROM dbo.AspNetUsers U
//    WHERE EXISTS(
//        SELECT 1
//        FROM dbo.AspNetUserRoles UR
//        INNER JOIN dbo.AspNetRoles R ON UR.RoleId = R.Id
//        WHERE UR.UserId = U.Id
//    )
//    AND NOT EXISTS(
//        SELECT 1
//        FROM dbo.AspNetUserRoles UR
//        INNER JOIN dbo.AspNetRoles R ON UR.RoleId = R.Id
//        WHERE UR.UserId = U.Id AND R.Name IN ('Admin', 'Manager')
//    );

//    IF @UserId IS NULL
//        RAISERROR('❌ Не знайдено користувача без ролей Admin або Manager.', 16, 1);


//        DECLARE @RequestId INT;
//    SELECT TOP 1 @RequestId = MR.Id
//    FROM dbo.MedicineRequests MR
//    WHERE MR.MedicineId = @MedicineId AND MR.Status = 2;

//        IF @RequestId IS NULL
//        RAISERROR('❌ Не знайдено запиту на ці ліки зі статусом 2.', 16, 1);


//        UPDATE dbo.MedicineRequests
    
//        SET Status = 3,
//            ApprovedByUserId = @UserId
    
//        WHERE Id = @RequestId;


//        PRINT '❌ Очікувана помилка НЕ виникла — тригер не спрацював.';

//    COMMIT TRANSACTION;
//        END TRY
//BEGIN CATCH
//    PRINT '✅ Очікувана помилка від тригера: ' + ERROR_MESSAGE();
//        ROLLBACK TRANSACTION;
//        END CATCH;



        public async Task CreateUpdateExpiredTendersProcedureAsync()
        {
            string script = @"
            IF OBJECT_ID('sp_UpdateExpiredTenders', 'P') IS NOT NULL
                DROP PROCEDURE sp_UpdateExpiredTenders;

            EXEC('
            CREATE PROCEDURE sp_UpdateExpiredTenders
            AS
            BEGIN
                SET NOCOUNT ON;

                UPDATE T
                SET T.Status = 3
                FROM Tenders T
                WHERE T.Status = 2
                  AND T.DeadlineDate < GETUTCDATE();
            END
            ')
            ";

            await _unitOfWork.ExecuteSqlRawAsync(script);
        }

        public async Task CreateUpdateExpiredMedicineRequestsProcedureAsync()
        {
                    string script = @"
            IF OBJECT_ID('sp_UpdateExpiredMedicineRequests', 'P') IS NOT NULL
                DROP PROCEDURE sp_UpdateExpiredMedicineRequests;

            EXEC('
            CREATE PROCEDURE sp_UpdateExpiredMedicineRequests
            AS
            BEGIN
                SET NOCOUNT ON;

                UPDATE MR
                SET MR.Status = 4
                FROM MedicineRequests MR
                WHERE MR.Status IN (1, 2)
                  AND MR.RequiredByDate < GETUTCDATE();
            END
            ')
            ";

            await _unitOfWork.ExecuteSqlRawAsync(script);
        }

        public async Task CreateDailyJobForExpiredUpdatesAsync()
        {
            string script = @"
    IF NOT EXISTS (SELECT 1 FROM msdb.dbo.sysjobs WHERE name = 'Daily_UpdateExpiredEntities')
    BEGIN

        EXEC msdb.dbo.sp_add_job
            @job_name = N'Daily_UpdateExpiredEntities',
            @enabled = 1,
            @description = N'Updates expired tenders and medicine requests daily',
            @start_step_id = 1;

        EXEC msdb.dbo.sp_add_jobstep
            @job_name = N'Daily_UpdateExpiredEntities',
            @step_name = N'UpdateExpiredTenders',
            @subsystem = N'TSQL',
            @command = N'EXEC sp_UpdateExpiredTenders;',
            @on_success_action = 3, -- go to next step
            @on_fail_action = 2; -- quit with failure

        EXEC msdb.dbo.sp_add_jobstep
            @job_name = N'Daily_UpdateExpiredEntities',
            @step_name = N'UpdateExpiredMedicineRequests',
            @subsystem = N'TSQL',
            @command = N'EXEC sp_UpdateExpiredMedicineRequests;',
            @on_success_action = 1, -- quit with success
            @on_fail_action = 2; -- quit with failure

        EXEC msdb.dbo.sp_add_schedule
            @schedule_name = N'DailyMidnightSchedule',
            @freq_type = 4,  -- daily
            @freq_interval = 1,
            @active_start_time = 000000; -- at midnight

        EXEC msdb.dbo.sp_attach_schedule
            @job_name = N'Daily_UpdateExpiredEntities',
            @schedule_name = N'DailyMidnightSchedule';

        EXEC msdb.dbo.sp_add_jobserver
            @job_name = N'Daily_UpdateExpiredEntities';
    END
    ";

            await _unitOfWork.ExecuteSqlRawAsync(script);
        }

    }
}
