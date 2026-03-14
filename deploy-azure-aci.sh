#!/bin/bash

# Azure Container Instances Deployment Script
# Deploys Deep Learning Protocol Web App to Azure ACI

set -e

# Configuration
RESOURCE_GROUP="deeplearningprotocol-rg"
LOCATION="eastus"
CONTAINER_NAME="deeplearningprotocol-web"
IMAGE_NAME="deeplearningprotocol-web"
IMAGE_TAG="latest"
DNS_NAME_LABEL="deeplearningprotocol-web-$(date +%s)"

echo "🚀 Deploying Deep Learning Protocol Web App to Azure Container Instances"
echo "================================================================="

# Login to Azure (uncomment if needed)
# echo "Logging in to Azure..."
# az login

# Create resource group if it doesn't exist
echo "📁 Creating resource group: $RESOURCE_GROUP"
az group create --name $RESOURCE_GROUP --location $LOCATION --output none

# Build and push Docker image to Azure Container Registry
echo "🐳 Building Docker image..."
docker build -f Dockerfile.web -t $IMAGE_NAME:$IMAGE_TAG .

# Create Azure Container Registry if it doesn't exist
ACR_NAME="deeplearningprotocolacr"
echo "📦 Creating Azure Container Registry: $ACR_NAME"
az acr create --resource-group $RESOURCE_GROUP --name $ACR_NAME --sku Basic --output none

# Login to ACR
echo "🔐 Logging in to Azure Container Registry..."
az acr login --name $ACR_NAME

# Tag and push image
ACR_LOGIN_SERVER=$(az acr show --name $ACR_NAME --query loginServer -o tsv)
FULL_IMAGE_NAME="$ACR_LOGIN_SERVER/$IMAGE_NAME:$IMAGE_TAG"

echo "🏷️  Tagging image: $FULL_IMAGE_NAME"
docker tag $IMAGE_NAME:$IMAGE_TAG $FULL_IMAGE_NAME

echo "📤 Pushing image to ACR..."
docker push $FULL_IMAGE_NAME

# Deploy to Azure Container Instances
echo "🚀 Deploying to Azure Container Instances..."
az container create \
    --resource-group $RESOURCE_GROUP \
    --name $CONTAINER_NAME \
    --image $FULL_IMAGE_NAME \
    --cpu 1 \
    --memory 1.5 \
    --ports 80 443 \
    --dns-name-label $DNS_NAME_LABEL \
    --environment-variables ASPNETCORE_URLS="http://+:80;https://+:443" \
    --output none

# Get the public IP
echo "🌐 Getting deployment details..."
FQDN=$(az container show --resource-group $RESOURCE_GROUP --name $CONTAINER_NAME --query ipAddress.fqdn -o tsv)

echo ""
echo "✅ Deployment completed successfully!"
echo "================================================================="
echo "🌐 Web App URL: https://$FQDN"
echo "📊 Check status: az container show --resource-group $RESOURCE_GROUP --name $CONTAINER_NAME"
echo "🛑 To stop: az container delete --resource-group $RESOURCE_GROUP --name $CONTAINER_NAME"
echo "================================================================="