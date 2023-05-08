using ChatGptLikeBlazorApp.Services.Interfaces.Data;

namespace ChatGptLikeBlazorApp.Services.Interfaces;
public interface IChatService
{
    /// <summary>
    /// 現状のチャットスレッドを元に OpenAI に問い合わせを行い戻ってきた結果のメッセージを targetChatThread に追加します。
    /// チャットメッセージ取得後に chats の内容を保存します。
    /// </summary>
    /// <param name="chats">チャットスレッドの所属するチャット</param>
    /// <param name="targetChatThread">チャットスレッド</param>
    /// <returns></returns>
    ValueTask AskToAgentAsync(Chats chats, CancellationToken cancellationToken = default);

    ValueTask ResetChatsAsync(Chats chats, CancellationToken cancellationToken = default);

    /// <summary>
    /// 引数で渡されたユーザーのチャットを取得します。存在しない場合は作成します。
    /// </summary>
    /// <param name="user">ユーザー情報</param>
    /// <returns>そのユーザーのチャット</returns>
    ValueTask<Chats> GetOrCreateChatForUserAsync(OwnerInfo user, CancellationToken cancellationToken = default);
}
