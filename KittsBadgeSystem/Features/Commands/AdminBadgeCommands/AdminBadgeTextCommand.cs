using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace KittsBadgeSystem.Features.Commands.AdminBadgeCommands;

[CommandHandler(typeof(AdminBadgeParentCommand))]
internal sealed class AdminBadgeTextCommand : ICommand
{
    public string Command => "text";
    public string[] Aliases => ["tx", "te", "tt"];
    public string Description => "Set badge text";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count < 2)
        {
            response = $"Usage: abadge text <player> <text>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        BadgeData data = player.GetBadgeData();

        data.BadgeText = string.Join(" ", arguments.Skip(1));

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Badge text updated to {data.BadgeText}.";

        Log.Debug("AdminBadgeTextCommand", $"{Player.Get(sender)?.DisplayName} changed {player.DisplayName}");
        return true;
    }
}