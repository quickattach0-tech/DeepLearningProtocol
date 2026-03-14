#!/bin/bash

# AWS Fargate Deployment Script
# Deploys Deep Learning Protocol Web App to AWS ECS Fargate

set -e

# Configuration
CLUSTER_NAME="deeplearningprotocol-cluster"
SERVICE_NAME="deeplearningprotocol-web-service"
TASK_FAMILY="deeplearningprotocol-web-task"
CONTAINER_NAME="deeplearningprotocol-web"
IMAGE_NAME="deeplearningprotocol-web"
IMAGE_TAG="latest"

echo "🚀 Deploying Deep Learning Protocol Web App to AWS Fargate"
echo "========================================================="

# Check if AWS CLI is configured
if ! aws sts get-caller-identity &> /dev/null; then
    echo "❌ AWS CLI is not configured. Please run 'aws configure' first."
    exit 1
fi

# Get AWS account ID and region
AWS_ACCOUNT_ID=$(aws sts get-caller-identity --query Account --output text)
AWS_REGION=$(aws configure get region)
if [ -z "$AWS_REGION" ]; then
    AWS_REGION="us-east-1"
    echo "⚠️  No default region set, using us-east-1"
fi

# Build Docker image
echo "🐳 Building Docker image..."
docker build -f Dockerfile.web -t $IMAGE_NAME:$IMAGE_TAG .

# Create ECR repository if it doesn't exist
REPO_NAME="deeplearningprotocol-web"
echo "📦 Creating ECR repository: $REPO_NAME"
aws ecr describe-repositories --repository-names $REPO_NAME --region $AWS_REGION &> /dev/null || \
aws ecr create-repository --repository-name $REPO_NAME --region $AWS_REGION --output text > /dev/null

# Get ECR login and login
echo "🔐 Logging in to Amazon ECR..."
aws ecr get-login-password --region $AWS_REGION | docker login --username AWS --password-stdin $AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com

# Tag and push image
ECR_URI="$AWS_ACCOUNT_ID.dkr.ecr.$AWS_REGION.amazonaws.com/$REPO_NAME:$IMAGE_TAG"
echo "🏷️  Tagging image: $ECR_URI"
docker tag $IMAGE_NAME:$IMAGE_TAG $ECR_URI

echo "📤 Pushing image to ECR..."
docker push $ECR_URI

# Create ECS cluster if it doesn't exist
echo "📊 Creating ECS cluster: $CLUSTER_NAME"
aws ecs describe-clusters --clusters $CLUSTER_NAME --region $AWS_REGION | jq -e '.clusters[0]' &> /dev/null || \
aws ecs create-cluster --cluster-name $CLUSTER_NAME --region $AWS_REGION --output text > /dev/null

# Create task definition
echo "📋 Creating task definition..."
cat > task-definition.json << EOF
{
    "family": "$TASK_FAMILY",
    "taskRoleArn": "",
    "executionRoleArn": "",
    "networkMode": "awsvpc",
    "requiresCompatibilities": ["FARGATE"],
    "cpu": "256",
    "memory": "512",
    "containerDefinitions": [
        {
            "name": "$CONTAINER_NAME",
            "image": "$ECR_URI",
            "essential": true,
            "portMappings": [
                {
                    "containerPort": 80,
                    "protocol": "tcp"
                },
                {
                    "containerPort": 443,
                    "protocol": "tcp"
                }
            ],
            "environment": [
                {
                    "name": "ASPNETCORE_URLS",
                    "value": "http://+:80;https://+:443"
                }
            ],
            "logConfiguration": {
                "logDriver": "awslogs",
                "options": {
                    "awslogs-group": "/ecs/$TASK_FAMILY",
                    "awslogs-region": "$AWS_REGION",
                    "awslogs-stream-prefix": "ecs"
                }
            }
        }
    ]
}
EOF

aws ecs register-task-definition --cli-input-json file://task-definition.json --region $AWS_REGION

# Create CloudWatch log group
echo "📝 Creating CloudWatch log group..."
aws logs create-log-group --log-group-name "/ecs/$TASK_FAMILY" --region $AWS_REGION 2> /dev/null || true

# Note: For a complete deployment, you would also need to:
# 1. Create a VPC, subnets, and security groups
# 2. Create an Application Load Balancer
# 3. Create the ECS service

echo ""
echo "✅ Task definition created successfully!"
echo "========================================================="
echo "📋 Task Family: $TASK_FAMILY"
echo "🐳 Image: $ECR_URI"
echo "📊 Cluster: $CLUSTER_NAME"
echo ""
echo "📖 Next steps:"
echo "1. Create VPC, subnets, and security groups"
echo "2. Create Application Load Balancer"
echo "3. Create ECS service:"
echo "   aws ecs create-service --cluster $CLUSTER_NAME --service-name $SERVICE_NAME --task-definition $TASK_FAMILY --desired-count 1 --launch-type FARGATE --network-configuration 'awsvpcConfiguration={subnets=[subnet-12345,subnet-67890],securityGroups=[sg-12345],assignPublicIp=ENABLED}' --load-balancers 'targetGroupArn=arn:aws:elasticloadbalancing:region:account:targetgroup/my-targets/123456789,containerName=$CONTAINER_NAME,containerPort=80' --region $AWS_REGION"
echo "========================================================="

# Cleanup
rm -f task-definition.json