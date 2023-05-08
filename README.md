# ChatGptLikeBlazorApp

## How to use

1. Create following resources to Azure:
- Azure OpenAI Service
  - Deploy a ChatGPT 3.5 turbo model.
- CosmosDB
  - Create a database what name is `chats-db`.
  - Create a collection what name is `chats` with the `/owner/uniqueId` partition key.
- Azure App Service
2. Configure App Service authentication with an AzureAD single-tenant app.
3. Add settings
Please add the fllowing settings to application settings of App Service.
- `OpenAI__Endpoint`: the endpoint of Azure OpenAI Service.
- `CosmosDb__ConnectionString`: the connection string of CosmosDB.
- `OpenAIAssistantClientOptions__ModelDeployName`: the model deploy name of Azure OpenAI Service.
- `ChatServiceOptions__DefaultSystemPrompt`: The default system prompt for the chat.
- `AzureAd__ClientId`: Your app id.
- `AzureAd__ClientSecret`: Your app secret.
- `AzureAd__TenantId`: Your tenant id.
- `AzureAd__Instance`: `https://login.microsoftonline.com/`
4. Add a `Cognitive Service OpenAI User` permission to the App Service's managed identity.
5. Deploy the `ChatGptLikeBlazorApp` project to the App Service.

### Options

If you want to more secure architecture, please add virtual network and private endpoints etc....
This app can run on virtual network.
