using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Enums;
using KittsBadgeSystem.Features.Models;
using KittsBadgeSystem.Features.Types;
using LabApi.Features.Wrappers;
using MEC;
using System.Collections.Generic;
using UnityEngine;

namespace KittsBadgeSystem.Features;

internal sealed class PlayerBadgeManager
{
    public static readonly PlayerBadgeManager Instance = new();

    private readonly Dictionary<Player, int> _rainbowPositions = [];
    private readonly HashSet<Player> _managedPlayers = [];

    public readonly Dictionary<Player, float> VisibilityChanged = [];

    private CoroutineHandle _handle;
    private float _nextCheck;
    private bool _running;

    private const float UpdateIntervalSeconds = 0.5f;

    public void Start()
    {
        if (_running)
            return;

        _running = true;

        Timing.OnPreExecute += CheckCoroutine;

        _handle = Timing.RunCoroutine(Run());

        Log.Debug($"PlayerBadgeManager.Start", "Started");
    }

    public void Stop()
    {
        if (!_running)
            return;

        Timing.OnPreExecute -= CheckCoroutine;

        _rainbowPositions.Clear();
        _managedPlayers.Clear();
        VisibilityChanged.Clear();

        _running = false;

        Timing.OnPreExecute -= CheckCoroutine;

        Timing.KillCoroutines(_handle);

        Log.Debug($"PlayerBadgeManager.Stop", "Stopped");
    }

    private void CheckCoroutine()
    {
        if (!_running || Time.unscaledTime < _nextCheck)
            return;

        _nextCheck = Time.unscaledTime + 1f;

        if (!Timing.IsRunning(_handle))
        {
            Log.Warn("PlayerBadgeManager", "Coroutine stopped unexpectedly. Restarting.");

            _handle = Timing.RunCoroutine(Run());
        }
    }

    private IEnumerator<float> Run()
    {
        while (_running)
        {
            yield return Timing.WaitForSeconds(UpdateIntervalSeconds);
            yield return Timing.WaitUntilDone(Update());
        }
    }

    private IEnumerator<float> Update()
    {
        foreach (Player player in Player.ReadyList)
        {
            BadgeData data = player.GetBadgeData();
            ServerRoles serverRoles = player.ReferenceHub.serverRoles;

            if (VisibilityChanged.TryGetValue(player, out float changedAt))
            {
                if (Time.time - changedAt < 1f)
                {
                    yield return Timing.WaitForOneFrame;
                    continue;
                }

                VisibilityChanged.Remove(player);

                if (!serverRoles.HasBadgeHidden)
                {
                    if (data.BadgeEnabled)
                        _managedPlayers.Add(player);
                    else
                    {
                        serverRoles.SetText(player.UserGroup.BadgeText);
                        serverRoles.SetColor(player.UserGroup.BadgeColor);

                        _managedPlayers.Remove(player);
                    }
                }
            }

            if (serverRoles.HasBadgeHidden)
            {
                yield return Timing.WaitForOneFrame;
                continue;
            }

            if (!data.BadgeEnabled)
            {
                if (_managedPlayers.Remove(player))
                {
                    serverRoles.SetText(player.UserGroup.BadgeText);
                    serverRoles.SetColor(player.UserGroup.BadgeColor);
                }

                yield return Timing.WaitForOneFrame;
                continue;
            }

            _managedPlayers.Add(player);

            string badgeText = string.IsNullOrWhiteSpace(data.BadgeText)
                ? KittsBadgeSystem.Config.DefaultBadgeFormat
                    .Replace("{group}", player.UserGroup.BadgeText)
                : KittsBadgeSystem.Config.BadgeFormat
                    .Replace("{group}", player.UserGroup.BadgeText)
                    .Replace("{text}", data.BadgeText);

            serverRoles.SetText(badgeText);
            serverRoles.SetColor(GetBadgeColour(player, data));

            yield return Timing.WaitForOneFrame;
        }
    }

    private string GetBadgeColour(Player player, BadgeData data)
    {
        if (data.BadgeColour == Colour.Clear)
            return player.UserGroup.BadgeColor;

        if (data.RainbowEnabled)
        {
            if (!_rainbowPositions.ContainsKey(player))
                _rainbowPositions[player] = 0;

            int pos = (_rainbowPositions[player] + 1) % ColourTypes.RainbowColours.Count;
            _rainbowPositions[player] = pos;

            return ColourTypes.RainbowColours[pos];
        }

        return ColourTypes.Get(data.BadgeColour).InternalName;
    }
}
