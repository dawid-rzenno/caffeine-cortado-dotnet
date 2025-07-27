using cortado.Models;

namespace cortado.DTOs;

public class NutrientDetails : Nutrient
{
    public MassUnit MassUnit { get; set; }
    public NutrientType Type { get; set; }
    public NutrientName Name { get; set; }
    
    public NutrientDetails() {}
    
    public NutrientDetails(Nutrient nutrient, NutrientName name, NutrientType type, MassUnit massUnit)
    {
        Id = nutrient.Id;
        Name = name;
        Type = type;
        MassUnit = massUnit;
        UserId = nutrient.UserId;
        Timestamp = nutrient.Timestamp;
    }
}