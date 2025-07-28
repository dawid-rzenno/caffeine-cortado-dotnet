using cortado.Models;

namespace cortado.DTOs;

public class NutrientDetails : Nutrient
{
    public NutrientTypeDetails Type { get; set; }
    
    public NutrientDetails() {}
    
    public NutrientDetails(Nutrient nutrient, NutrientTypeDetails type)
    {
        Id = nutrient.Id;
        Type = type;
        Amount = nutrient.Amount;
        UserId = nutrient.UserId;
        Timestamp = nutrient.Timestamp;
    }
}