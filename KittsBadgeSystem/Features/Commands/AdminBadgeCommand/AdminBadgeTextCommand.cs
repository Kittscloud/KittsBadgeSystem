using CommandSystem;
using KittsBadgeSystem.Features.Database;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace KittsBadgeSystem.Features.Commands.AdminBadgeCommand;

[CommandHandler(typeof(AdminBadgeParentCommand))]
internal sealed class AdminBadgeTextCommand : ICommand
{
    public string Command => "text";
    public string[] Aliases => ["tx", "te", "tt"];
    public string Description => "Set your badge text";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 0)
        {
            response = $"Usage: abadge text <player> <text>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        player.GetBadgeData().BadgeText = string.Join(" ", arguments.Skip(1));

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = "Badge text updated.";

        Log.Debug("AdminBadgeTextCommand", $"{Player.Get(sender)?.DisplayName} changed {player.DisplayName}");
        return true;
    }
}