namespace ResumeRanker.Models;

public class JobRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string JobDescription { get; set; }
    public bool IsProcessed { get; set; } = false;
    public List<RankedResume> Resumes { get; set; } = [];
}
