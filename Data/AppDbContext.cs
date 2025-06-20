using Microsoft.EntityFrameworkCore;
using ResumeRanker.Models;

namespace ResumeRanker.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<JobRequest> JobRequests { get; set; }
    public DbSet<RankedResume> RankedResumes { get; set; }
}
