using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeRanker.Data;
using ResumeRanker.Models;
using ResumeRanker.ML;
using ModelBuilder = ResumeRanker.ML.ModelBuilder;

namespace ResumeRanker.Controllers;

[ApiController]
[Route("api/upload")]
public class UploadController(AppDbContext db, ModelBuilder modelBuilder) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Upload([FromBody] JobRequest job)
    {
        if (string.IsNullOrWhiteSpace(job.JobDescription) || job.Resumes == null || job.Resumes.Count == 0)
        {
            return BadRequest("JobDescription and at least one resume are required.");
        }

        // Trim text and set navigation property
        foreach (var resume in job.Resumes)
        {
            resume.Text = resume.Text?.Trim() ?? string.Empty;
            resume.JobRequest = job;
        }

        // Save job first to get JobRequest.Id
        db.JobRequests.Add(job);
        await db.SaveChangesAsync();

        // Load ML model and prediction engine
        var predictionEngine = modelBuilder.LoadModel();

        // Score each resume using ML.NET
        foreach (var resume in job.Resumes)
        {
            var input = new ResumeData
            {
                JobDescription = job.JobDescription,
                ResumeText = resume.Text
            };
            var prediction = predictionEngine.Predict(input);
            resume.Score = prediction.Probability; // Use the probability as the score
        }

        // Save updated scores
        await db.SaveChangesAsync();

        return Ok(new { job.Id });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetResults(Guid id)
    {
        var job = await db.JobRequests
            .Include(j => j.Resumes)
            .FirstOrDefaultAsync(j => j.Id == id);

        if (job == null) return NotFound();

        var ranked = job.Resumes.OrderByDescending(r => r.Score);
        return Ok(ranked);
    }
}
