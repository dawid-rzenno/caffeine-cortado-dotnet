using cortado.Models;

namespace cortado.DTOs;

public class NutrientDetails : Nutrient
{
    public MassUnit MassUnit { get; set; }
    public NutrientType Type { get; set; }
    
    public NutrientDetails() {}
    
    public NutrientDetails(Nutrient nutrient, NutrientType type, MassUnit massUnit)
    {
        Id = nutrient.Id;
        Type = type;
        MassUnit = massUnit;
        UserId = nutrient.UserId;
        Timestamp = nutrient.Timestamp;
    }
}