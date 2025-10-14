-- =============================================
-- Object: ufn_GetTraining  (SQL_INLINE_TABLE_VALUED_FUNCTION)
-- Generated: 2025-10-14 16:14:02
-- =============================================
CREATE FUNCTION dbo.ufn_GetTraining(
    @Id INT,
    @UserId INT
)
    RETURNS TABLE
        AS
        RETURN(SELECT Training.*,
                      TrainingExercise.Id         as TrainingExerciseId,
                      TrainingExercise.TrainingId as TrainingExerciseTrainingId,
                      TrainingExercise.ExerciseId as TrainingExerciseExerciseId,
                      TrainingExercise.UserId     as TrainingExerciseUserId,
                      TrainingExercise.Timestamp  as TrainingExerciseTimestamp,
                      Exercise.Id                 as ExerciseId,
                      Exercise.Name               as ExerciseName,
                      Exercise.UserId             as ExerciseUserId,
                      Exercise.Timestamp          as ExerciseTimestamp
               FROM Training
                        LEFT JOIN TrainingExercise ON Training.Id = TrainingExercise.TrainingId
                        LEFT JOIN Exercise ON TrainingExercise.ExerciseId = Exercise.Id
               WHERE Training.Id = @Id
                 AND (Training.UserId = @UserId OR Training.IsPublic = 1))
