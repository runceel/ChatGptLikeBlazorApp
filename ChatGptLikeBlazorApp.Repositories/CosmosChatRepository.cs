using ChatGptLikeBlazorApp.Repositories.Interfaces;
using ChatGptLikeBlazorApp.Services.Interfaces.Data;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

namespace ChatGptLikeBlazorApp.Repositories;
public class CosmosChatRepository : IChatRepository
{
    private const string s_databaseName = "chats-db";
    private const string s_chatsContainerName = "chats";
    private readonly CosmosClient _cosmosClient;

    public CosmosChatRepository(CosmosClient cosmosClient)
    {
        _cosmosClient = cosmosClient;
    }

    public async ValueTask<Chats?> GetChatsForUserAsync(OwnerInfo user, CancellationToken cancellationToken = default)
    {
        var chatsContainer = _cosmosClient.GetDatabase(s_databaseName).GetContainer(s_chatsContainerName);
        var partitionKey = new PartitionKey(user.UniqueId);
        using var iter = chatsContainer.GetItemLinqQueryable<Chats>(requestOptions: new() { PartitionKey = partitionKey })
            .Where(x => x.Owner.UniqueId == user.UniqueId)
            .ToFeedIterator();
        if (iter.HasMoreResults)
        {
            var result = await iter.ReadNextAsync(cancellationToken);
            return result.Resource.FirstOrDefault();
        }
        else
        {
            return null;
        }
    }

    public async ValueTask SaveAsync(Chats chats, CancellationToken cancellationToken = default)
    {
        var chatsContainer = _cosmosClient.GetDatabase(s_databaseName).GetContainer(s_chatsContainerName);
        var partitionKey = new PartitionKey(chats.Owner.UniqueId);
        await chatsContainer.UpsertItemAsync(chats, partitionKey);
    }
}
