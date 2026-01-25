# Multi-stage build Dockerfile for Deep Learning Protocol
# Optimized for minimal image size and runtime performance

# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS builder

WORKDIR /src

# Copy solution and project files
COPY DeepLearningProtocol.sln .
COPY Directory.Build.props .
COPY DeepLearningProtocol/DeepLearningProtocol.csproj ./DeepLearningProtocol/
COPY DeepLearningProtocol.Tests/DeepLearningProtocol.Tests.csproj ./DeepLearningProtocol.Tests/

# Restore dependencies
RUN dotnet restore DeepLearningProtocol.sln

# Copy source code
COPY . .

# Build the application
RUN dotnet build DeepLearningProtocol.sln --configuration Release --no-restore

# Run tests to ensure quality
RUN dotnet test DeepLearningProtocol.sln --configuration Release --no-build --verbosity=normal

# Publish the main application
RUN dotnet publish DeepLearningProtocol/DeepLearningProtocol.csproj \
    --configuration Release \
    --no-build \
    --output /app/publish


# Stage 2: Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:10.0-alpine

WORKDIR /app

# Create directory for DLP backups
RUN mkdir -p .dlp_backups

# Copy published application from builder stage
COPY --from=builder /app/publish .

# Add labels for metadata
LABEL maintainer="Deep Learning Protocol Contributors"
LABEL description="Deep Learning Protocol - A hierarchical multi-interface reasoning system with Data Loss Prevention"
LABEL version="1.0.0"

# Set entry point
ENTRYPOINT ["dotnet", "DeepLearningProtocol.dll"]

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
    CMD dotnet DeepLearningProtocol.dll --version || exit 1
