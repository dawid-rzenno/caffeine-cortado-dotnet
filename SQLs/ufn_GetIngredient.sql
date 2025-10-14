-- =============================================
-- Object: ufn_GetIngredient  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetIngredient(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT Ingredient.*,
                      IngredientNutrient.Id           as IngredientNutrientId,
                      IngredientNutrient.IngredientId as IngredientNutrientIngredientId,
                      IngredientNutrient.NutrientId   as IngredientNutrientNutrientId,
                      IngredientNutrient.Timestamp    as IngredientNutrientTimestamp,
                      IngredientNutrient.UserId       as IngredientNutrientUserId,
                      Nutrient.Id                     as NutrientId,
                      Nutrient.Amount                 as NutrientAmount,
                      Nutrient.NutrientTypeId         as NutrientNutrientTypeId,
                      Nutrient.Timestamp              as NutrientTimestamp,
                      Nutrient.UserId                 as NutrientUserId,
                      NutrientType.Id                 as NutrientTypeId,
                      NutrientType.Name               as NutrientTypeName,
                      NutrientType.MassUnitId         as NutrientTypeMassUnitId,
                      NutrientType.Timestamp          as NutrientTypeTimestamp,
                      NutrientType.UserId             as NutrientTypeUserId,
                      MassUnit.Id                     as MassUnitId,
                      MassUnit.Name                   as MassUnitName,
                      MassUnit.UserId                 as MassUnitUserId,
                      MassUnit.Timestamp              as MassUnitTimestamp
               FROM Ingredient
                        LEFT JOIN IngredientNutrient ON IngredientNutrient.IngredientId = Ingredient.Id
                        LEFT JOIN Nutrient ON IngredientNutrient.NutrientId = Nutrient.Id
                        LEFT JOIN NutrientType ON Nutrient.NutrientTypeId = NutrientType.Id
                        LEFT JOIN MassUnit ON NutrientType.MassUnitId = MassUnit.Id
               WHERE Ingredient.Id = @Id
                 AND (Ingredient.UserId = @UserId OR Ingredient.IsPublic = 1))
