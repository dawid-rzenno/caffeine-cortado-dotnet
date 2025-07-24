using cortado.Models;

namespace cortado.DTOs;

public class DashboardResponse
{
    public MotivationalQuote? MotivationalQuote { get; set; }
    public DietDetails? Diet { get; set; }
    public TrainingDetails? Training { get; set; }
    public IEnumerable<Goal> Goals { get; set; }

    public DashboardResponse(MotivationalQuote? motivationalQuote, DietDetails? diet, TrainingDetails? training, IEnumerable<Goal> goals)
    {
        MotivationalQuote = motivationalQuote;
        Diet = diet;
        Training = training;
        Goals = goals;
    }
}