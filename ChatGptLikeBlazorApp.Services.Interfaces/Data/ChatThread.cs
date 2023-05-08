namespace ChatGptLikeBlazorApp.Services.Interfaces.Data;

public class ChatThread
{
    public string Title { get; set; } = "";
    public ChatMessage SystemMessage { get; } = new() { Role = ChatRole.System };
    private List<ChatMessage> _chatMessages = new();
    public IReadOnlyCollection<ChatMessage> ChatMessages => _chatMessages;
    public void AddChatMessage(ChatMessage message)
    {
        message.CreatedAt ??= DateTimeOffset.UtcNow;
        _chatMessages.Add(message);
    }

    public void Reset(string systemPrompt)
    {
        SystemMessage.Message = systemPrompt;
        _chatMessages.Clear();
    }
}
