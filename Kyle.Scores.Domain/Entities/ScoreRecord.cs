using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kyle.Scores.Domain.Entities;

[Table("ScoreRecord")]
public class ScoreRecord
{
    public ScoreRecord()
    {
    }

    public ScoreRecord(byte type, int score, string reason)
    {
        Id = Guid.NewGuid();
        CreationTime = DateTime.Now;
        Type = type;
        Score = score;
        Reason = reason;
    }

    [Key]
    public Guid Id { get; set; }

    public DateTime CreationTime { get; set; }

    public byte Type { get; set; }

    public int Score { get; set; }

    public string Reason { get; set; }
}