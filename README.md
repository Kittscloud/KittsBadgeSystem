# KittsBadgeSystem
*LabAPI Badge Tool*

[![License](https://img.shields.io/badge/License-AGPL%20v3.0-blue?style=for-the-badge)](https://github.com/Kittscloud/KittsBadgeSystem/blob/main/LICENSE) [![Downloads](https://img.shields.io/github/downloads/Kittscloud/KittsBadgeSystem/total?style=for-the-badge)](https://github.com/Kittscloud/ServerSpecificsSyncer/releases/latest) [![GitHub release](https://img.shields.io/github/v/release/Kittscloud/KittsBadgeSystem?style=for-the-badge)](https://github.com/Kittscloud/KittsBadgeSystem/releases/latest) [![](https://img.shields.io/badge/.NET-4.8.1-512BD4?logo=dotnet&logoColor=fff&style=for-the-badge)](https://dotnet.microsoft.com/en-us/download/dotnet-framework/net481) [![GitHub stars](https://img.shields.io/github/stars/Kittscloud/KittsBadgeSystem?style=for-the-badge)](https://github.com/Kittscloud/KittsBadgeSystem/stargazers) [![GitHub issues](https://img.shields.io/github/issues/Kittscloud/KittsBadgeSystem?style=for-the-badge)](https://github.com/Kittscloud/KittsBadgeSystem/issues)

`KittsBadgeSystem` is a tool that controls badges for `SCP Secret Laboratory` using `LabAPI`.

## Consider Supporting?
If you enjoy this project and would like to support future development, I would greatly appreciate it if you considered donating via my [`Ko-Fi`](https://ko-fi.com/kittscloud).

## Installation
`KittsBadgeSystem` is available in two editions:

- **Standard**: Stores all infractions in a local JSON file.
- **MongoDB**: Stores all infractions in a MongoDB database.

All required files can be found in the [`latest release`](https://github.com/Kittscloud/KittsBadgeSystem/releases/latest).

### Standard
**Required files**
- `KittsBadgeSystem.dll` (latest version)
- `Newtonsoft.Json.dll` (`v13.0.4` or later)

**Installation**
- Place `KittsBadgeSystem.dll` in your `plugins` folder.
- Place `Newtonsoft.Json.dll` in your `dependencies` folder.

### MongoDB
**Required files**
- `KittsBadgeSystem-MongoDB.dll` (latest version)
- `DnsClient.dll` (`v1.6.1` or later)
- `Microsoft.Extensions.Logging.Abstractions.dll` (`v2.0.0` or later)
- `MongoDB.Bson.dll` (`v3.12.0` or later)
- `MongoDB.Driver.dll` (`v3.12.0` or later)

**Installation**
- Place `KittsBadgeSystem-MongoDB.dll` in your `plugins` folder.
- Place all other `.dll` files in your `dependencies` folder.

Run the server and you're set!

### Configurations:
| Parameter                   | Type     | Description                                                            | Default Value                            |
|-----------------------------|----------|------------------------------------------------------------------------|------------------------------------------|
| `Debug`                     | `bool`   | Sends debug logs to console.                                           | `false`                                  |
| `MongoDBURI`                | `string` | MongoDB URI.                                                           | `"mongodb://username:password@ip:port/"` |
| `MongoDBName`               | `string` | MongoDB name.                                                          | `KittsBadgeSystem`                       |
| `MongoDBCollectionName`     | `string` | MongoDB collection name if using database.                             | `BadgeData`                              |
| `PlayerBadgePermission`     | `string` | Permission for player badge command.                                   | `kts.badge`                              |
| `AdminBadgePermission`      | `string` | Permission for admin badge command.                                    | `kts.badgeadmin`                         |
| `DefaultBadgeFormat`        | `string` | Formatting for badges with no text, {group} = groupName.               | `'{group}'`                              |
| `BadgeFormat`               | `string` | Permission for admin badge command.                                    | `'{group} | {text}'`                     |

### Default YML Config File
```yml
# Sends debug logs to console
debug: false
# Permission for player badge command
player_badge_permission: kts.badge
# Permission for admin badge command
admin_badge_permission: kts.badgeadmin
# Formatting for badges with no text, {group} = groupName
default_badge_format: '{group}'
# Formatting for badges, {group} = groupName, {text} = badgeText
badge_format: '{group} | {text}'
```

### MongoDB YML Config File
```yml
# Sends debug logs to console
debug: false
# MongoDB URI
mongo_d_b_u_r_i: mongodb://username:password@ip:port/
# MongoDB name
mongo_d_b_name: KittsBadgeSystem
# MongoDB collection name
mongo_d_b_collection_name: BadgeData
# Permission for player badge command
player_badge_permission: kts.badge
# Permission for admin badge command
admin_badge_permission: kts.badgeadmin
# Formatting for badges with no text, {group} = groupName
default_badge_format: '{group}'
# Formatting for badges, {group} = groupName, {text} = badgeText
badge_format: '{group} | {text}'
```

### Want to use in your own project?
To install in your project, simply reference the `KittsBadgeSystem.dll` file, found in the [`latest release`](https://github.com/Kittscloud/KittsBadgeSystem/releases/latest).

Alternatively, you can use the `NuGet Package` which can be found here:
- [`KittsBadgeSystem`](https://www.nuget.org/packages/KittsBadgeSystem)
- [`KittsBadgeSystem-MongoDB`](https://www.nuget.org/packages/KittsBadgeSystem-MongoDB)

`KittsBadgeSystem.dll` is really easy to use, if you would like to change someone's badge data you can do `Player.GetBadgeData()` and change what you want.
You can also use the command if you're only using this as a standalone plugin.

A really simple example of using this plugin could be the colour being changed to a random colour when jumping as shown below.

```cssharp
public override void OnPlayerJumped(PlayerJumpedEventArgs ev)
{
    ev.Player.GetBadgeData().BadgeColour = ColourTypes.Random().EnumColour;

    // If you're not using MonogoDB you will need to update the JsonCache as shown below
    DatabaseJson.SaveJsonCache();
    // If you are using MongoDB it will automatically update to the database
}
```

## Found a bug or have feedback?
If you have found a bug please make an issue on GitHub or the quickest way is to message me on discord at `kittscloud`.

Also message me on discord if you have feedback for me, I'd appreciate it very much. Thank you!
