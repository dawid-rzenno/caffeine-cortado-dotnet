using cortado.Models;

namespace cortado.DTOs;

public class DietDetails : Diet
{
    public IEnumerable<Meal> Meals { get; set; }

    public DietDetails(Diet diet, IEnumerable<Meal> meals)
    {
        Id = diet.Id;
        Name = diet.Name;
        Meals = meals;
    }
}