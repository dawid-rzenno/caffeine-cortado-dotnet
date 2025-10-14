using cortado.Models;

namespace cortado.DTOs;

public class GoalDetails : Goal
{
    public IList<Milestone> Milestones { get; set; }

    public GoalDetails(Goal goal, IList<Milestone> milestones)
    {
        Id = goal.Id;
        Name = goal.Name;
        Milestones = milestones;
    }
}