namespace ResumeRanker.ML;

public class ResumeData
{
    public required string ResumeText { get; set; }
    public required string JobDescription { get; set; }
    public bool IsRelevant { get; set; } // only needed for training
}
