namespace Kyle.Scores.Messages;

public class ScoreChangeMessage
{
    public int Score { get; set; }

    public byte Type { get; set; }

    public string Reason { get; set; }
}