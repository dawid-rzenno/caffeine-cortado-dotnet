using cortado.Models;

namespace cortado.DTOs;

public class IngredientDetails : Ingredient
{
    public IEnumerable<Nutrient> Nutrients { get; set; }

    public IngredientDetails(Ingredient ingredient, IEnumerable<Nutrient> nutrients)
    {
        Id = ingredient.Id;
        Name = ingredient.Name;
        Nutrients = nutrients;
    }
}