using Microsoft.ML;

namespace ResumeRanker.ML;

public class ModelBuilder
{
    private static readonly string ModelPath = "resume_model.zip";
    private readonly MLContext _mlContext = new();

    // Sample training data method
    public static IEnumerable<ResumeData> GetSampleTrainingData()
    {
        return
        [
            new ResumeData
            {
                JobDescription = "Looking for a .NET backend engineer with ML.NET experience",
                ResumeText = "Experienced C# developer with knowledge of ML.NET and backend services",
                IsRelevant = true
            },
            new ResumeData
            {
                JobDescription = "Looking for a .NET backend engineer with ML.NET experience",
                ResumeText = "Frontend React developer with 5 years of experience",
                IsRelevant = false
            },
            new ResumeData
            {
                JobDescription = "Looking for a .NET backend engineer with ML.NET experience",
                ResumeText = "Skilled in Java, Python and machine learning frameworks",
                IsRelevant = true
            },
            new ResumeData
            {
                JobDescription = "Looking for a .NET backend engineer with ML.NET experience",
                ResumeText = "Graphic designer with 10 years of experience",
                IsRelevant = false
            }
        ];
    }

    public ITransformer BuildModel(IEnumerable<ResumeData> trainingData)
    {
        var data = _mlContext.Data.LoadFromEnumerable(trainingData);

        var pipeline = _mlContext.Transforms.Text.FeaturizeText("ResumeFeatures", nameof(ResumeData.ResumeText))
            .Append(_mlContext.Transforms.Text.FeaturizeText("JobFeatures", nameof(ResumeData.JobDescription)))
            .Append(_mlContext.Transforms.Concatenate("Features", "ResumeFeatures", "JobFeatures"))
            .Append(_mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                labelColumnName: nameof(ResumeData.IsRelevant),
                featureColumnName: "Features"));

        var model = pipeline.Fit(data);
        _mlContext.Model.Save(model, data.Schema, ModelPath);

        return model;
    }

    // Load the model from disk and create a PredictionEngine
    public PredictionEngine<ResumeData, ResumePrediction> LoadModel()
    {
        var model = _mlContext.Model.Load(ModelPath, out _);
        return _mlContext.Model.CreatePredictionEngine<ResumeData, ResumePrediction>(model);
    }
}
