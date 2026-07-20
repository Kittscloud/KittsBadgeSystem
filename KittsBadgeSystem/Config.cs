using System.ComponentModel;

namespace KittsBadgeSystem;

public class Config
{
    /// <summary>
    /// Sends debug logs to console.
    /// </summary>
    [Description("Sends debug logs to console")]
    public bool Debug { get; set; } = false;

#if MONGODB
    /// <summary>
    /// MongoDB URI.
    /// </summary>
    [Description("MongoDB URI")]
    public string MongoDBURI { get; set; } = "mongodb://username:password@ip:port/";

    /// <summary>
    /// MongoDB name.
    /// </summary>
    [Description("MongoDB name")]
    public string MongoDBName { get; set; } = "KittsBadgeSystem";

    /// <summary>
    /// MongoDB collection name.
    /// </summary>
    [Description("MongoDB collection name")]
    public string MongoDBCollectionName { get; set; } = "BadgeData";
#endif

    /// <summary>
    /// Permission for player badge command.
    /// </summary>
    [Description("Permission for player badge command")]
    public string PlayerBadgePermission { get; set; } = "kts.badge";

    /// <summary>
    /// Permission for admin badge command.
    /// </summary>
    [Description("Permission for admin badge command")]
    public string AdminBadgePermission { get; set; } = "kts.badgeadmin";

    /// <summary>
    /// Formatting for badges with no text, {group} = groupName.
    /// </summary>
    [Description("Formatting for badges with no text, {group} = groupName")]
    public string DefaultBadgeFormat { get; set; } = "{group}";

    /// <summary>
    /// Formatting for badges, {group} = groupName, {text} = badgeText.
    /// </summary>
    [Description("Formatting for badges, {group} = groupName, {text} = badgeText")]
    public string BadgeFormat { get; set; } = "{group} | {text}";
}
