using ChatGptLikeBlazorApp.Services.Interfaces.Data;

namespace ChatGptLikeBlazorApp.Repositories.Interfaces;
public interface IAssistantClient
{
    ValueTask<ChatMessage> GetNextAgentChatMessageAsync(Chats chats, CancellationToken cancellationToken = default);
}
