# 🧠 AI-Powered Resume Ranker

This project is a backend service built with **C#**, **ASP.NET Core**, and **ML.NET** that uses machine learning to score and rank resumes based on relevance to a given job description.

## 🚀 Features

- Upload a job description and a list of resumes
- ML.NET model predicts relevance between each resume and the job
- Ranks resumes by score (probability of relevance)
- Entity Framework Core for database access
- Runs locally and in Docker
- Designed for scalability with background job support

## 🛠️ Tech Stack

- **C# / ASP.NET Core** (.NET 9)
- **ML.NET** – text featurization and binary classification
- **Entity Framework Core**
- **Docker** – for containerized deployment

## 📦 Getting Started

1. Clone the repo
   ```bash
   git clone https://github.com/iNoles/ResumeRanker.git
   cd ResumeRanker
   ```
2. Run locally
    ```bash
    dotnet run
    ```
3. Run with Docker
   ```bash
   docker build -t ResumeRanker .
   docker run -p 8080:80 ResumeRanker
   ```
