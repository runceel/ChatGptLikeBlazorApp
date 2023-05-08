namespace ChatGptLikeBlazorApp.Services.Interfaces.Data;

public class OwnerInfo
{
    public static OwnerInfo Empty { get; } = new OwnerInfo();

    public string DisplayName { get; set; } = "";
    public string ObjectId { get; set; } = "";
    public string TenantId { get; set; } = "";
    public string UniqueId => (TenantId, ObjectId) switch
    {
        ({}, {}) => $"{TenantId}|{ObjectId}",
        _ => "",
    };

    public override bool Equals(object? obj)
    {
        if (obj is OwnerInfo other)
        {
            return UniqueId == other.UniqueId;
        }

        return false;
    }

    public override int GetHashCode() => UniqueId.GetHashCode();

    public static bool operator ==(OwnerInfo a, OwnerInfo b) => a.Equals(b);
    public static bool operator !=(OwnerInfo a, OwnerInfo b) => !a.Equals(b);
}
