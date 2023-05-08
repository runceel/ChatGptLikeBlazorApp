namespace ChatGptLikeBlazorApp.Services.Interfaces.Data;

public class ChatMessage
{
    public string Message { get; set; } = "";
    public ChatRole Role { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
}
