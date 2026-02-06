# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY FitJournal.sln ./
COPY src/FitJournal.Core/FitJournal.Core.csproj src/FitJournal.Core/
COPY src/FitJournal.Application/FitJournal.Application.csproj src/FitJournal.Application/
COPY src/FitJournal.Infrastructure/FitJournal.Infrastructure.csproj src/FitJournal.Infrastructure/
COPY src/FitJournal.Web/FitJournal.Web.csproj src/FitJournal.Web/

# Restore dependencies
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish src/FitJournal.Web/FitJournal.Web.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render uses PORT environment variable
ENV ASPNETCORE_URLS=http://+:${PORT:-10000}
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 10000
ENTRYPOINT ["dotnet", "FitJournal.Web.dll"]
