using DotNetCore.CAP;
using Kyle.Extensions;
using Kyle.Infrastructure.CAP;
using Kyle.Scores.Domain;
using Kyle.Scores.Domain.Entities;

namespace Kyle.Scores.ProcessManagers;

public class ScoreSubscribeService: ISubscribeTransientDependency, ICapSubscribe
{
    private readonly IScoreOperationRepository _repository;

    public ScoreSubscribeService(IScoreOperationRepository repository)
    {
        _repository = repository;
    }

    [CapSubscribe(CAPMessageNamedConst.MESSAGE_NAME)]
    public async Task ChangeScore(ScoreChangeMessage message)
    {
        await _repository.Insert(new ScoreRecord(message.Type,message.Score,message.Reason));
    }
}
public class ScoreChangeMessage
{
    public int Score { get; set; }

    public byte Type { get; set; }

    public string Reason { get; set; }
}