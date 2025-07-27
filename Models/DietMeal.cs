namespace cortado.Models;

public class DietMeal : Entity
{
    public int DietId { get; set; }
    public int MealId { get; set; }
    public int MealIndex { get; set; }
    public int MealDayIndex { get; set; }
}