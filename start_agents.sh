#!/bin/bash

# Start DLP Agent
echo "Starting DLP Agent..."
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj agent DLP &
DLP_PID=$!

# Wait a moment for DLP to connect
sleep 2

# Start Analyzer Agent
echo "Starting Analyzer Agent..."
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj agent Analyzer &
ANALYZER_PID=$!

echo "Both agents started. PIDs: DLP=$DLP_PID, Analyzer=$ANALYZER_PID"
echo "Press Ctrl+C to stop all agents"

# Wait for user interrupt
trap "echo 'Stopping agents...'; kill $DLP_PID $ANALYZER_PID 2>/dev/null; exit" INT
wait