USE [Cortado];

-- ID of user that we impersonate during data migration.
DECLARE @UserId AS INT = 1025;
DECLARE @CurrentDate AS DATETIME = GETDATE();

BEGIN TRANSACTION;
BEGIN TRY
    -- ID for iteration.
    DECLARE @CurrentId INT;

    DECLARE NutritionCursor CURSOR FOR
        SELECT Id FROM [temp].[Nutrition] ORDER BY Id;

    OPEN NutritionCursor;
    FETCH NEXT FROM NutritionCursor INTO @CurrentId;

    -- Fetch status equal to 0 means cursor points at a row that is not NULL.
    WHILE @@FETCH_STATUS = 0
        BEGIN
            -- Meal name.
            DECLARE @Name AS NVARCHAR(255);

            -- Its nutrients.
            DECLARE @Calories AS DECIMAL(12,2);
            DECLARE @Proteins AS DECIMAL(12,2);
            DECLARE @Carbohydrates AS DECIMAL(12,2);
            DECLARE @Fats AS DECIMAL(12,2);

            -- Fetch row data to split into separate INSERT-s.
            SELECT
                @Name = Name,
                @Calories = Calories,
                @Proteins = Proteins,
                @Carbohydrates = Carbohydrates,
                @Fats = Fats
            FROM [temp].[Nutrition] WHERE Id = @CurrentId



            INSERT INTO [dbo].[Ingredients] (Name, Timestamp, UserId)
            VALUES (@Name, @CurrentDate, @UserId)

            DECLARE @IngredientId AS INT = SCOPE_IDENTITY();

            INSERT INTO [dbo].[Nutrients] (TypeId, Amount, Timestamp, UserId)
            VALUES (1, @Calories, @CurrentDate, @UserId)
            INSERT INTO [dbo].[IngredientNutrients] (IngredientId, NutrientId, UserId, Timestamp)
            VALUES (@IngredientId, SCOPE_IDENTITY(), @UserId, @CurrentDate)

            INSERT INTO [dbo].[Nutrients] (TypeId, Amount, Timestamp, UserId)
            VALUES (2, @Proteins, @CurrentDate, @UserId)
            INSERT INTO [dbo].[IngredientNutrients] (IngredientId, NutrientId, UserId, Timestamp)
            VALUES (@IngredientId, SCOPE_IDENTITY(), @UserId, @CurrentDate)


            INSERT INTO [dbo].[Nutrients] (TypeId, Amount, Timestamp, UserId)
            VALUES (3, @Carbohydrates, @CurrentDate, @UserId)
            INSERT INTO [dbo].[IngredientNutrients] (IngredientId, NutrientId, UserId, Timestamp)
            VALUES (@IngredientId, SCOPE_IDENTITY(), @UserId, @CurrentDate)


            INSERT INTO [dbo].[Nutrients] (TypeId, Amount, Timestamp, UserId)
            VALUES (4, @Fats, @CurrentDate, @UserId)
            INSERT INTO [dbo].[IngredientNutrients] (IngredientId, NutrientId, UserId, Timestamp)
            VALUES (@IngredientId, SCOPE_IDENTITY(), @UserId, @CurrentDate)


            FETCH NEXT FROM NutritionCursor INTO @CurrentId;
        END;

    CLOSE NutritionCursor;
    DEALLOCATE NutritionCursor;

    COMMIT TRANSACTION;
END TRY
BEGIN CATCH
    PRINT 'An error occurred.';
    PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR);
    PRINT 'Error Message: ' + ERROR_MESSAGE();
    PRINT 'Error Line: ' + CAST(ERROR_LINE() AS VARCHAR);

    CLOSE NutritionCursor;
    DEALLOCATE NutritionCursor;

    ROLLBACK TRANSACTION;
END CATCH