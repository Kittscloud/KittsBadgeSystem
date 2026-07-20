using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.AdminBadgeCommands;

internal sealed class AdminBadgeResetCommand : ICommand
{
    public string Command => "Reset";
    public string[] Aliases => [];
    public string Description => "Reset badge";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = $"Usage: abadge reset <player>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        BadgeData data = player.GetBadgeData();

        data.BadgeEnabled = false;
        data.BadgeColour = Enums.Colour.Clear;
        data.BadgeText = "";

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Reset {player.DisplayName}'s badge";

        Log.Debug("AdminBadgeResetCommand", Player.Get(sender)?.DisplayName);
        return true;
    }
}