using cortado.Models;

namespace cortado.DTOs;

public class GoalDetails : Goal
{
    public IEnumerable<Milestone> Milestones { get; set; }

    public GoalDetails(Goal goal, IEnumerable<Milestone> milestones)
    {
        Id = goal.Id;
        Name = goal.Name;
        Milestones = milestones;
    }
}