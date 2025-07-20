CREATE TABLE DietDayDiet (
    DietDayId INT NOT NULL,
    DietId INT NOT NULL,
    CONSTRAINT FK_DietDayDiet_DietDayId FOREIGN KEY (DietDayId) REFERENCES DietDays(Id),
    CONSTRAINT FK_DietDayDiet_DietId FOREIGN KEY (DietId) REFERENCES Diets(Id),
    CONSTRAINT UQ_DietDayDiet_DietDayId_DietId UNIQUE (DietDayId, DietId)
);

CREATE TABLE DietMeal (
    DietId INT NOT NULL,
    MealId INT NOT NULL,
    CONSTRAINT FK_DietMeal_DietId FOREIGN KEY (DietId) REFERENCES Diets(Id),
    CONSTRAINT FK_DietMeal_MealId FOREIGN KEY (MealId) REFERENCES Meals(Id),
    CONSTRAINT UQ_DietMeal_DietId_MealId UNIQUE (DietId, MealId)
);

CREATE TABLE MealIngredient (
    MealId INT NOT NULL,
    IngredientId INT NOT NULL,
    CONSTRAINT FK_MealIngredient_MealId FOREIGN KEY (MealId) REFERENCES Meals(Id),
    CONSTRAINT FK_MealIngredient_IngredientId FOREIGN KEY (IngredientId) REFERENCES Ingredients(Id),
    CONSTRAINT UQ_MealIngredient_MealId_IngredientId UNIQUE (MealId, IngredientId)
);

CREATE TABLE IngredientNutrient (
    IngredientId INT NOT NULL,
    NutrientId INT NOT NULL,
    CONSTRAINT FK_IngredientNutrient_IngredientId FOREIGN KEY (IngredientId) REFERENCES Ingredients(Id),
    CONSTRAINT FK_IngredientNutrient_NutrientId FOREIGN KEY (NutrientId) REFERENCES Nutrients(Id),
    CONSTRAINT UQ_IngredientNutrient_IngredientId_NutrientId UNIQUE (IngredientId, NutrientId)
);