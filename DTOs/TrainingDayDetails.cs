using cortado.Models;

namespace cortado.DTOs;

public class TrainingDayDetails : Training
{
    public IEnumerable<Exercise> Exercises { get; set; }

    public TrainingDayDetails(TrainingDay trainingDay, IEnumerable<Exercise> exercises)
    {
        Id = trainingDay.Id;
        Name = trainingDay.Name;
        UserId = trainingDay.UserId;
        Timestamp = trainingDay.Timestamp;
        Exercises = exercises;
    }
}