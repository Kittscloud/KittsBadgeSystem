#if MONGODB
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using MongoDB.Driver;
using System.Collections.Generic;

namespace KittsBadgeSystem.Features.Database;

public static class DatabaseMongo
{
    private static MongoClient _client;
    private static IMongoDatabase _db;
    private static IMongoCollection<BadgeData> _playerBadgeDataCollection;

    public static void Init()
    {
        _client = new MongoClient(KittsBadgeSystem.Config.MongoDBURI);
        _db = _client.GetDatabase(KittsBadgeSystem.Config.MongoDBName);
        _playerBadgeDataCollection = _db.GetCollection<BadgeData>(KittsBadgeSystem.Config.MongoDBCollectionName);

        CreateIndexes();
    }

    private static void CreateIndexes()
    {
        _playerBadgeDataCollection.Indexes.CreateOne(
            new CreateIndexModel<BadgeData>(
                Builders<BadgeData>.IndexKeys.Ascending(p => p.UserId),
                new CreateIndexOptions { Unique = true }
            )
        );
    }

    public static void Stop()
    {
        _playerBadgeDataCollection = null;
        _client = null;
        _db = null;
    }

    private static readonly Dictionary<string, BadgeData> _cache = [];

    public static IReadOnlyDictionary<string, BadgeData> Cache => _cache;

    public static BadgeData LoadBadgeData(string userId)
    {
        BadgeData data = _playerBadgeDataCollection
            .Find(p => p.UserId == userId)
            .FirstOrDefault();

        if (data == null)
        {
            data = new() { UserId = userId };

            _playerBadgeDataCollection.InsertOne(data);
        }

        _cache[userId] = data;

        Log.Debug("Database.LoadBadgeData", data.UserId);

        return data;
    }
    public static BadgeData LoadBadgeData(this Player player) =>
        LoadBadgeData(player.UserId);
    public static BadgeData LoadBadgeData(this ReferenceHub hub) =>
        Player.Get(hub).LoadBadgeData();

    public static void UploadBadgeData(this Player player)
    {
        BadgeData badgeData = player.GetBadgeData();

        if (badgeData == null)
            return;

        _playerBadgeDataCollection.ReplaceOne(
            p => p.UserId == badgeData.UserId,
            badgeData,
            new ReplaceOptions { IsUpsert = true }
        );

        Log.Debug("Database.UploadBadgeData", $"{player.UserId} | {player.DisplayName}");
    }
    public static BadgeData GetBadgeData(this Player player)
    {
        if (_cache.TryGetValue(player.UserId, out BadgeData cached) && cached != null)
            return cached;

        return player.LoadBadgeData();
    }
    public static BadgeData GetBadgeData(this ReferenceHub hub) =>
        Player.Get(hub).GetBadgeData();
}
#endif
