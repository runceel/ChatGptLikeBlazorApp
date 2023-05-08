using ChatGptLikeBlazorApp.Services.Interfaces.Data;

namespace ChatGptLikeBlazorApp.Repositories.Interfaces;
public interface IChatRepository
{
    ValueTask<Chats?> GetChatsForUserAsync(OwnerInfo user, CancellationToken cancellationToken = default);
    ValueTask SaveAsync(Chats chats, CancellationToken cancellationToken = default);
}
