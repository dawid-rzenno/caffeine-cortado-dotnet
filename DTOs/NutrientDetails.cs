using cortado.Models;

namespace cortado.DTOs;

public class NutrientDetails : Nutrient
{
    public MassUnit MassUnit { get; set; }
    
    public NutrientDetails(Nutrient ingredient, MassUnit massUnit)
    {
        Id = ingredient.Id;
        Name = ingredient.Name;
        MassUnit = massUnit;
    }
}