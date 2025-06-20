using System.Text.Json.Serialization;

namespace ResumeRanker.Models;

public class RankedResume
{
    public int Id { get; set; }

    // Resume content
    public required string Text { get; set; }

    // ML score from 0 to 1
    public float Score { get; set; }

    // Foreign key to JobRequest
    public Guid JobRequestId { get; set; }

    // Navigation property to parent
    [JsonIgnore]
    public JobRequest? JobRequest { get; set; }
}