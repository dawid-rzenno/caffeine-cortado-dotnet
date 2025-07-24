using cortado.Models;

namespace cortado.DTOs;

public class TrainingDetails : Training
{
    public IEnumerable<Exercise> Exercises { get; set; }

    public TrainingDetails(Training training, IEnumerable<Exercise> exercises)
    {
        Id = training.Id;
        Name = training.Name;
        UserId = training.UserId;
        Timestamp = training.Timestamp;
        Exercises = exercises;
    }
}