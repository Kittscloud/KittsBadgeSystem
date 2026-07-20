using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Enums;
using KittsBadgeSystem.Features.Models;
using KittsBadgeSystem.Features.Types;
using LabApi.Features.Wrappers;
using MEC;
using System.Collections.Generic;

namespace KittsBadgeSystem.Features;

internal sealed class PlayerBadgeManager
{
    public static readonly PlayerBadgeManager Instance = new();

    private readonly Dictionary<Player, int> _rainbowPositions = [];

    private CoroutineHandle _handle;
    private bool _running;

    private const float UpdateIntervalSeconds = 1f;

    public void Start()
    {
        if (_running)
            return;

        _running = true;
        _handle = Timing.RunCoroutine(Run());

        Log.Debug($"PlayerBadgeManager.Start", "Started");
    }

    public void Stop()
    {
        if (!_running)
            return;

        _rainbowPositions.Clear();

        Timing.KillCoroutines(_handle);
        _running = false;

        Log.Debug($"PlayerBadgeManager.Stop", "Stopped");
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

            string badgeText = string.IsNullOrWhiteSpace(data.BadgeText)
                ? KittsBadgeSystem.Config.DefaultBadgeFormat
                    .Replace("{group}", player.UserGroup.BadgeText)
                : KittsBadgeSystem.Config.BadgeFormat
                    .Replace("{group}", player.UserGroup.BadgeText)
                    .Replace("{text}", data.BadgeText);

            if (player.ReferenceHub.serverRoles.HasBadgeHidden)
            {
                player.ReferenceHub.serverRoles.HiddenBadge = data.BadgeEnabled
                    ? badgeText
                    : string.Empty;

                player.ReferenceHub.serverRoles.RefreshHiddenTag();
            }
            else
            {
                if (data.BadgeEnabled)
                {
                    player.ReferenceHub.serverRoles.Network_myText = badgeText;

                    player.ReferenceHub.serverRoles.Network_myColor = GetBadgeColour(player, data);
                }
                else
                {
                    player.ReferenceHub.serverRoles.Network_myText = player.UserGroup.BadgeText;

                    player.ReferenceHub.serverRoles.Network_myColor = player.UserGroup.BadgeColor;
                }
            }

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
