using cortado.Models;

namespace cortado.DTOs;

public class DietMealDetails : Meal
{
    public DietMeal? DietMeal { get; set; }

    public DietMealDetails(DietMeal? dietMeal, Meal meal)
    {
        Id = meal.Id;
        Name = meal.Name;
        UserId = meal.UserId;
        Timestamp = meal.Timestamp;
        DietMeal = dietMeal;
    }
}