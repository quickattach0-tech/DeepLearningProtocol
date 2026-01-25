#!/bin/bash
# Test script to verify translator functionality

echo "Testing Translator Module..."
echo ""

# Run the translator tests via the binary
cd /workspaces/DeepLearningProtocol

# Build first
echo "Building application..."
dotnet build -q

echo ""
echo "=== Translator Feature Test Results ==="
echo ""

# Test 1: Can we translate a simple phrase?
echo "Test 1: Basic Translation"
echo "Expected: Spanish, Arabic, French translations should work"
dotnet run --project DeepLearningProtocol/DeepLearningProtocol.csproj << EOF
3
4
5
q
EOF

echo ""
echo "=== Test Complete ==="
