using cortado.Models;

namespace cortado.DTOs;

public class DashboardDetails
{
    public MotivationalQuote? MotivationalQuote { get; set; }
    public DietDetails? Diet { get; set; }
    public TrainingDetails? Training { get; set; }
    public IEnumerable<Goal> Goals { get; set; }

    public DashboardDetails(MotivationalQuote? motivationalQuote, DietDetails? diet, TrainingDetails? training, IEnumerable<Goal> goals)
    {
        MotivationalQuote = motivationalQuote;
        Diet = diet;
        Training = training;
        Goals = goals;
    }
}