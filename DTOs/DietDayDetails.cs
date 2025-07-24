using cortado.Models;

namespace cortado.DTOs;

public class DietDayDetails : DietDay
{
    public IEnumerable<Meal> Meals { get; set; }

    public DietDayDetails(DietDay dietDay, IEnumerable<Meal> meals)
    {
        Id = dietDay.Id;
        Name = dietDay.Name;
        Meals = meals;
    }
}