using cortado.Models;

namespace cortado.DTOs;

public class TrainingDetails : Training
{
    public IEnumerable<TrainingDay> Days { get; set; }

    public TrainingDetails(Training training, IEnumerable<TrainingDay> days)
    {
        Id = training.Id;
        Name = training.Name;
        UserId = training.UserId;
        Timestamp = training.Timestamp;
        Days = days;
    }
}