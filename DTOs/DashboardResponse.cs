using cortado.Models;

namespace cortado.DTOs;

public class DashboardResponse
{
    public MotivationalQuote? MotivationalQuote { get; set; }
    public DietDay? DietDay { get; set; }
    public TrainingDay? TrainingDay { get; set; }
    public IEnumerable<Goal> Goals { get; set; }

    public DashboardResponse(MotivationalQuote? motivationalQuote, DietDay? dietDay, TrainingDay? trainingDay, IEnumerable<Goal> goals)
    {
        MotivationalQuote = motivationalQuote;
        DietDay = dietDay;
        TrainingDay = trainingDay;
        Goals = goals;
    }
}