#if !MONGODB
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using LabApi.Loader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KittsBadgeSystem.Features.Database;

public static class DatabaseJson
{
    private static List<BadgeData> _jsonCache = [];
    private static string JsonFilePath => Path.Combine(
        KittsBadgeSystem.Instance.GetConfigDirectory().FullName, "BadgeData.json");

    public static void Init() =>
        LoadJsonCache();

    public static void Stop()
    {
        SaveJsonCache();
        _jsonCache.Clear();
    }

    private static void EnsureJsonFile()
    {
        string dir = KittsBadgeSystem.Instance.GetConfigDirectory().FullName;

        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);

        if (!File.Exists(JsonFilePath))
            File.WriteAllText(JsonFilePath, "[]");
    }

    private static void LoadJsonCache()
    {
        try
        {
            EnsureJsonFile();

            List<BadgeData> jsonCache = JsonConvert.DeserializeObject<List<BadgeData>>(File.ReadAllText(JsonFilePath));
            if (jsonCache != null)
                _jsonCache = jsonCache;
        }
        catch (Exception e)
        {
            Log.Error("DatabaseJson.LoadJsonCache", $"Error loading JSON cache: {e.Message}");
            Log.Debug("DatabaseJson.LoadJsonCache", e.ToString());
        }
    }

    public static void SaveJsonCache()
    {
        try
        {
            File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(_jsonCache, Formatting.Indented));
        }
        catch (Exception e)
        {
            Log.Error("DatabaseJson.SaveJsonCache", $"Error saving JSON cache: {e.Message}");
            Log.Debug("DatabaseJson.SaveJsonCache", e.ToString());
        }
    }

    public static BadgeData GetBadgeData(string userId)
    {
        BadgeData data = _jsonCache
            .Where(d => d.UserId == userId)
            .FirstOrDefault();

        if (data == null)
        {
            data = new()
            {
                UserId = userId
            };

            _jsonCache.Add(data);
        }

        return data;
    }

    public static BadgeData GetBadgeData(this ReferenceHub hub) =>
        GetBadgeData(Player.Get(hub).UserId);

    public static BadgeData GetBadgeData(this Player player) =>
        GetBadgeData(player.UserId);
}
#endif
