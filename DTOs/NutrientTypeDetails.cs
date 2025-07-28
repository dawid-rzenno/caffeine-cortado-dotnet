using cortado.Models;

namespace cortado.DTOs;

public class NutrientTypeDetails : NutrientType
{
    public MassUnit MassUnit;
    
    public NutrientTypeDetails() {}

    public NutrientTypeDetails(MassUnit massUnit)
    {
        MassUnit = massUnit;
    }
}