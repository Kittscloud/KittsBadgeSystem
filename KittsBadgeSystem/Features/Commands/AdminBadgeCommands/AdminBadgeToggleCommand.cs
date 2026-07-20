using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using LabApi.Features.Wrappers;
using System;

namespace KittsBadgeSystem.Features.Commands.AdminBadgeCommands;

[CommandHandler(typeof(AdminBadgeParentCommand))]
internal sealed class AdminBadgeToggleCommand : ICommand
{
    public string Command => "toggle";
    public string[] Aliases => ["t", "to", "tg"];
    public string Description => "Toggle badge";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count != 1)
        {
            response = $"Usage: abadge toggle <player>";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        BadgeData data = player.GetBadgeData();

        data.BadgeEnabled = !data.BadgeEnabled;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Badge {(data.BadgeEnabled ? "enabled" : "disabled")}.";

        Log.Debug("AdminBadgeToggleCommand", $"{Player.Get(sender)?.DisplayName} changed {player.DisplayName}");
        return true;
    }
}