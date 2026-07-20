using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.AdminBadgeCommand;

[CommandHandler(typeof(AdminBadgeParentCommand))]
internal sealed class AdminBadgeRainbowCommand : ICommand
{
    public string Command => "rainbow";
    public string[] Aliases => ["r"];
    public string Description => "Toggle rainbow mode.";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = $"Usage: abadge rainbow <player>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        BadgeData badge = player.GetBadgeData();

        badge.RainbowEnabled = !badge.RainbowEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Rainbow {(badge.RainbowEnabled ? "enabled" : "disabled")}.";

        Log.Debug("AdminBadgeRainbowCommand", $"{Player.Get(sender)?.DisplayName} changed {player.DisplayName}");
        return true;
    }
}
