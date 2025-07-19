using cortado.Models;

namespace cortado.DTOs;

public class GoalResponse : Goal
{
    public IEnumerable<Milestone> Milestones { get; set; }

    public GoalResponse(Goal goal, IEnumerable<Milestone> milestones)
    {
        Id = goal.Id;
        Name = goal.Name;
        Milestones = milestones;
    }
}