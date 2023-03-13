using Kyle.Scores.Domain.Entities;

namespace Kyle.Scores.Domain;

public interface IScoreOperationRepository
{
    Task Insert(ScoreRecord model);
}