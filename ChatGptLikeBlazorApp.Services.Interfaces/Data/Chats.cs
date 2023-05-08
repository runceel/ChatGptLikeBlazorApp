namespace ChatGptLikeBlazorApp.Services.Interfaces.Data;

public class Chats
{
    public string Id { get; set; } = "";
    public ChatThread ChatThread { get; set; } = new();
    public OwnerInfo Owner { get; set; } = OwnerInfo.Empty;
}
