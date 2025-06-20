# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0.301 AS build
WORKDIR /app

# Copy project files and restore dependencies
COPY *.sln ./
COPY ResumeRanker/*.csproj ./ResumeRanker/
RUN dotnet restore

# Copy the rest of the app
COPY ResumeRanker/. ./ResumeRanker/

# Publish to /app/out
WORKDIR /app/ResumeRanker
RUN dotnet publish -c Release -o /app/out

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0.6
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/out .

# Expose app port
EXPOSE 80

# Run the app
ENTRYPOINT ["dotnet", "ResumeRanker.dll"]
