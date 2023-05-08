using ChatGptLikeBlazorApp.Repositories.Interfaces;
using ChatGptLikeBlazorApp.Services.Interfaces;
using ChatGptLikeBlazorApp.Services.Interfaces.Data;
using Microsoft.Extensions.Options;

namespace ChatGptLikeBlazorApp.Services;

public class ChatService : IChatService
{
    private readonly IChatRepository _chatRepository;
    private readonly IAssistantClient _openAIClient;
    private readonly IOptions<ChatServiceOptions> _chatServiceOptions;

    public ChatService(IChatRepository chatRepository, IAssistantClient openAIClient, IOptions<ChatServiceOptions> chatServiceOptions)
    {
        _chatRepository = chatRepository;
        _openAIClient = openAIClient;
        _chatServiceOptions = chatServiceOptions;
    }

    public async ValueTask<Chats> GetOrCreateChatForUserAsync(OwnerInfo user, CancellationToken cancellationToken = default)
    {
        var chats = await _chatRepository.GetChatsForUserAsync(user);
        if (chats is null)
        {
            chats = CreateDefaultChat(user);

            await _chatRepository.SaveAsync(chats, cancellationToken);
        }

        return chats;
    }

    private Chats CreateDefaultChat(OwnerInfo user)
    {
        var chats = new Chats
        {
            Id = Guid.NewGuid().ToString(),
            Owner = user,
        };
        chats.ChatThread.SystemMessage.Message = _chatServiceOptions.Value.DefaultSystemPrompt;
        return chats;
    }

    public async ValueTask AskToAgentAsync(Chats chats, CancellationToken cancellationToken = default)
    {
        var agentMessage = await _openAIClient.GetNextAgentChatMessageAsync(chats, cancellationToken);
        chats.ChatThread.AddChatMessage(agentMessage);
        await _chatRepository.SaveAsync(chats, cancellationToken);
    }

    public async ValueTask ResetChatsAsync(Chats chats, CancellationToken cancellationToken = default)
    {
        chats.ChatThread.Reset(_chatServiceOptions.Value.DefaultSystemPrompt);
        await _chatRepository.SaveAsync(chats, cancellationToken);
    }
}
