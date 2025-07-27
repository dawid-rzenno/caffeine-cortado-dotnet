using cortado.Models;

namespace cortado.DTOs;

public class DietDetails : Diet
{
    public IList<DietMealDetails> Meals { get; set; } =  new List<DietMealDetails>();
    
    public DietDetails() {}

    public DietDetails(Diet diet, IList<DietMealDetails> meals)
    {
        Id = diet.Id;
        Name = diet.Name;
        Meals = meals;
    }
}