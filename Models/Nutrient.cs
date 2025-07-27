namespace cortado.Models;

public class Nutrient : Entity
{
    public int NameId { get; set; }
    public int TypeId { get; set; }
    public decimal Amount { get; set; }
}