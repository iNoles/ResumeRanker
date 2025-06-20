using ResumeRanker.ML;
using ResumeRanker.Models;

namespace ResumeRanker.Services;

public class ResumeScoringService(ModelBuilder modelBuilder)
{
    private readonly ModelBuilder _modelBuilder = modelBuilder;

    public async Task ProcessJobAsync(JobRequest job)
    {
        var engine = _modelBuilder.LoadModel();

        foreach (var resume in job.Resumes)
        {
            var prediction = engine.Predict(new ResumeData
            {
                JobDescription = job.JobDescription,
                ResumeText = resume.Text
            });

            resume.Score = prediction.Probability;
        }

        await Task.CompletedTask;
    }
}
