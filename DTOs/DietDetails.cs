using cortado.Models;

namespace cortado.DTOs;

public class DietDetails : Diet
{
    public IEnumerable<DietDay> Days { get; set; }

    public DietDetails(Diet diet, IEnumerable<DietDay> days)
    {
        Id = diet.Id;
        Name = diet.Name;
        Days = days;
    }
}