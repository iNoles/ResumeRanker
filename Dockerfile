# -------- Build Stage --------
FROM mcr.microsoft.com/dotnet/sdk:9.0.6 AS build
WORKDIR /app

# Copy everything in the root
COPY . ./

# Restore and publish (assumes single .csproj in root)
RUN dotnet restore
RUN dotnet publish -c Release -o out

# -------- Runtime Stage --------
FROM mcr.microsoft.com/dotnet/aspnet:9.0.6
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "ResumeRanker.dll"]
