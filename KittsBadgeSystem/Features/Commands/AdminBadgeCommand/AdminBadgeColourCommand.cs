using CommandSystem;
using KittsBadgeSystem.Features.Database;
using KittsBadgeSystem.Features.Models;
using KittsBadgeSystem.Features.Types;
using LabApi.Features.Wrappers;
using System;
using System.Linq;

namespace KittsBadgeSystem.Features.Commands.AdminBadgeCommand;

[CommandHandler(typeof(AdminBadgeParentCommand))]
[CommandHandler(typeof(GameConsoleCommandHandler))]
internal sealed class AdminBadgeColourCommand : ICommand
{
    public string Command => "colour";
    public string[] Aliases => ["color", "c"];
    public string Description => "Set your badge colour";

    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (arguments.Count == 0 || arguments.Count > 2)
        {
            response = $"Usage: abadge colour <player> <colour>\nAvailable Colours: {string.Join(", ", ColourTypes.AllValid.Keys.Select(c => c.ToString().ToLower()))}";
            return false;
        }

        if (!int.TryParse(arguments.At(0), out int playerId) || !Player.TryGet(playerId, out Player player))
        {
            response = "Player not found";
            return false;
        }

        if (!ColourTypes.TryGet(arguments.At(1), out ColourInfo colour))
        {
            response = $"Unknown colour.\nAvailable colours: {string.Join(", ", ColourTypes.AllValid.Keys.Select(c => c.ToString().ToLower()))}";
            return false;
        }

        player.GetBadgeData().BadgeColour = colour.EnumColour;

#if !MONGODB
        DatabaseJson.SaveJsonCache();
#endif

        response = $"Badge colour set to {colour.DisplayName}.";

        Log.Debug("AdminBadgeColourCommand", $"{Player.Get(sender)?.DisplayName} changed {player.DisplayName}");
        return true;
    }
}
