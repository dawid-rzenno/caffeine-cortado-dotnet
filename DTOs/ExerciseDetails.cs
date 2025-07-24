using cortado.Models;

namespace cortado.DTOs;

public class ExerciseDetails : Exercise
{
    public ExerciseDetails(Exercise exercise)
    {
        Id = exercise.Id;
        Name = exercise.Name;
        UserId = exercise.UserId;
        Timestamp = exercise.Timestamp;
    }
}