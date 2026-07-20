#if MONGODB
using MongoDB.Bson.Serialization.Attributes;
#endif
using KittsBadgeSystem.Features.Enums;

namespace KittsBadgeSystem.Features.Models;

public sealed class BadgeData
{
#if MONGODB
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string ObjectId = null;
#endif

    public string UserId = "";

    public bool BadgeEnabled = true;

    public bool RainbowEnabled = false;
    public Colour BadgeColour = Colour.Clear;
    public string BadgeText = "";
}
