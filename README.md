# ChatGptLikeBlazorApp

## How to use

1. Create following resources to Azure:
- Azure OpenAI Service
  - Deploy a ChatGPT 3.5 turbo model.
- CosmosDB
  - Create a database what name is `chats-db`.
  - Create a collection what name is `chats` with the `/owner/uniqueId` partition key.
- Azure App Service

2. Add settings
Please add the fllowing settings to application settings of App Service.
- `OpenAI__Endpoint`: the endpoint of Azure OpenAI Service.
- `CosmosDb__ConnectionString`: the connection string of CosmosDB.
- `OpenAIAssistantClientOptions`: the model deploy name of Azure OpenAI Service.
- `ChatServiceOptions__DefaultSystemPrompt`: The default system prompt for the chat.

3. Configure app service authentication with the AzureAD single tenant app.
