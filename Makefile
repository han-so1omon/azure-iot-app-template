# Variables
RESOURCE_GROUP=myResourceGroup
LOCATION=eastus
TEMPLATE_FILE=azure-deployment.json
DEPLOYMENT_NAME=iotDeployment

# Define the default target
.PHONY: deploy

# Create the resource group if it doesn't exist
create-resource-group:
	az group create --name $(RESOURCE_GROUP) --location $(LOCATION)

# Deploy the ARM template
deploy: create-resource-group
	az deployment group create \
		--name $(DEPLOYMENT_NAME) \
		--resource-group $(RESOURCE_GROUP) \
		--template-file $(TEMPLATE_FILE)

# Clean up resources
.PHONY: clean
clean:
	az group delete --name $(RESOURCE_GROUP) --yes --no-wait
