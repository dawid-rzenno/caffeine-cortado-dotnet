using cortado.Models;

namespace cortado.DTOs;

public class MealDetails : Meal
{
    public IEnumerable<Ingredient> Ingredients { get; set; }

    public MealDetails(Meal meal, IEnumerable<Ingredient> ingredients)
    {
        Id = meal.Id;
        Name = meal.Name;
        Ingredients = ingredients;
    }
}