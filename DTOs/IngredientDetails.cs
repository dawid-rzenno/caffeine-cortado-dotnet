using cortado.Models;

namespace cortado.DTOs;

public class IngredientDetails : Ingredient
{
    public IList<NutrientDetails> Nutrients { get; set; }

    public IngredientDetails()
    {
    }

    public IngredientDetails(Ingredient ingredient, IList<NutrientDetails> nutrients)
    {
        Id = ingredient.Id;
        Name = ingredient.Name;
        Nutrients = nutrients;
    }
}