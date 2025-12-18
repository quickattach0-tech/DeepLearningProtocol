#!/bin/bash
cd "$(dirname "$0")"
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj "$@"
