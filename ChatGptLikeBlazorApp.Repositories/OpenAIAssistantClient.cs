using ChatGptLikeBlazorApp.Repositories.Interfaces;
using ChatGptLikeBlazorApp.Services.Interfaces.Data;
using Microsoft.DeepDev;
using Microsoft.Extensions.Options;
using AI = Azure.AI.OpenAI;
using AIChatRole = Azure.AI.OpenAI.ChatRole;

namespace ChatGptLikeBlazorApp.Repositories;
public class OpenAIAssistantClient : IAssistantClient
{
    private readonly AI.OpenAIClient _openAIClient;
    private readonly ITokenizer _tokenizer;
    private readonly IOptions<OpenAIAssistantClientOptions> _assistantClientOptions;
    private const int s_maxTokenForRequest = 7000;
    private const int s_maxTotalToken = 8000;

    public OpenAIAssistantClient(AI.OpenAIClient openAIClient, 
        ITokenizer tokenizer,
        IOptions<OpenAIAssistantClientOptions> assistantClientOptions)
    {
        _openAIClient = openAIClient;
        _tokenizer = tokenizer;
        _assistantClientOptions = assistantClientOptions;
    }

    public async ValueTask<ChatMessage> GetNextAgentChatMessageAsync(Chats chats, CancellationToken cancellationToken = default)
    {
        static AIChatRole convertAssistantChatRole(ChatRole chatRole)
        {
            return chatRole switch
            {
                ChatRole.Assistant => AIChatRole.Assistant,
                ChatRole.User => AIChatRole.User,
                _ => throw new InvalidOperationException(),
            };
        }

        static ChatRole convertToAppChatRole(AIChatRole chatRole)
        {
            return chatRole.Label switch
            {
                "assistant" => ChatRole.Assistant,
                "user" => ChatRole.User,
                _ => throw new InvalidOperationException(),
            };
        }

        var currentChatThread = chats.ChatThread;
        var options = new AI.ChatCompletionsOptions
        {
            Temperature = 0.3f,
        };

        var totalToken = 0;
        if (currentChatThread.SystemMessage is not null)
        {
            totalToken = _tokenizer.Encode(currentChatThread.SystemMessage.Message, Array.Empty<string>()).Count;
            options.Messages.Add(new(AIChatRole.System, currentChatThread.SystemMessage.Message));
        }

        var messages = new List<AI.ChatMessage>();
        foreach (var chatMessage in currentChatThread.ChatMessages.AsEnumerable().Reverse().Where(x => x.Role is ChatRole.Assistant or ChatRole.User))
        {
            totalToken += _tokenizer.Encode(chatMessage.Message, Array.Empty<string>()).Count;
            if (totalToken > s_maxTokenForRequest) { break; }

            messages.Insert(0, new(convertAssistantChatRole(chatMessage.Role), chatMessage.Message));
        }

        foreach (var x in messages)
        {
            options.Messages.Add(x);
        }
        options.MaxTokens = s_maxTotalToken - totalToken;
        var response = await _openAIClient.GetChatCompletionsAsync(
            _assistantClientOptions.Value.ModelDeployName,
            options,
            cancellationToken);
        var responseMessage = response.Value.Choices.FirstOrDefault();
        if (responseMessage is null)
        {
            return new ChatMessage
            {
                Role = ChatRole.Error,
                Message = "No response from the assistant.",
            };
        }

        return new()
        {
            Role = convertToAppChatRole(responseMessage.Message.Role),
            Message = responseMessage.Message.Content,
        };
    }
}
