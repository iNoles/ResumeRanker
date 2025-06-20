using Microsoft.EntityFrameworkCore;
using ResumeRanker.Background;
using ResumeRanker.Data;
using ResumeRanker.Services;
using ModelBuilder = ResumeRanker.ML.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core InMemory DB (replace with SQL Server for prod)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("ResumeDB"));

// Register ML ModelBuilder and build the model on startup
var modelBuilder = new ModelBuilder();
var trainingData = ModelBuilder.GetSampleTrainingData();
var mlModel = modelBuilder.BuildModel(trainingData);

builder.Services.AddSingleton(modelBuilder);
builder.Services.AddSingleton(mlModel);

// Register scoring service and background worker
builder.Services.AddScoped<ResumeScoringService>();
builder.Services.AddHostedService<ResumeProcessingWorker>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
