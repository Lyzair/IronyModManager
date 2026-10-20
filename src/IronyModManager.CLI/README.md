# IronyModManager.CLI

Headless access to Irony's conflict solver. `conflicts` runs the same definition parsing and priority evaluation as the GUI's read-only conflict solver. It prints every definition key that more than one mod defines, with the winning definition and the reason (`FIOS`, `LIOS`, `ModOrder`, `ModOverride`, `None`). The output is JSON for scripts and agents, or text for people.

The CLI is read-only. It never writes patch mods, collections, launcher files or the GUI's settings.

## Stateless by design

- No configuration files ship with the CLI, and it creates none. Its options are built in code.
- Each run uses a fresh storage folder `<temp>/IronyCLI-<guid>`. Game detection and anything else Irony's services persist lands there, and the folder is deleted when the run ends, error exits included.
- The conflict solver keeps definitions in memory (`UseHybridMemory` and `UseDiskSearch` are off).
- `--collection` reads the GUI's database by copying the newest `IronyModManager/Database_*.json` from the GUI storage into the run's temp folder. The GUI's own files are only read.
- Logs go to stderr (warnings and errors only), so stdout carries nothing but the report.
- The cost: `--include-game` indexes the base game on every run. That took about 6 minutes for Stellaris 4.5, compared with under a minute without it.

## Usage

```
IronyModManager.CLI games
IronyModManager.CLI conflicts --playlist <launcher-playlist.json> [--json] [--include-game] [--game Stellaris]
IronyModManager.CLI conflicts --collection <name> [--irony-storage <dir>] [--json] [--include-game] [--game Stellaris]
```

- `games`: lists the detected games with their user and workshop directories.
- `--playlist`: a Paradox Launcher playlist export, format v3: `{game, name, mods: [{displayName, enabled, position, steamId, pdxId?}]}`. Disabled entries are skipped. Load order is `position`. Entries are matched to installed mods by Steam ID, and local mods without a remote ID are matched by name.
- `--collection`: the name of an Irony GUI collection.
- `--irony-storage`: where to find the GUI's storage. The default is `<ApplicationData>/Mario`, which is `~/.config/Mario` on Linux.
- `--game`: a game type as printed by `games`. The default is `Stellaris`.
- `--include-game`: lets the base game's definitions take part.

### Exit codes

| Code | Meaning |
|------|---------|
| 0 | success |
| 1 | unexpected error (details on stderr) |
| 2 | bad input: unreadable playlist, unknown game or collection, missing GUI database, or not exactly one of `--playlist` / `--collection` |
| 3 | a mod is too large to parse (`ModTooLargeException`) |

### JSON output

```json
{
  "game": "Stellaris",
  "mods": ["<mod name in load order>", "..."],
  "unmatched": [{ "displayName": "...", "enabled": true, "position": 3, "steamId": "...", "pdxId": null }],
  "conflicts": [
    {
      "typeAndId": "common/buildings/txt-building_clone_vats",
      "type": "common/buildings/txt",
      "id": "building_clone_vats",
      "valueType": "Object",
      "priorityType": "LIOS",
      "winner": { "mod": "More Multiple Origins", "file": "common/buildings/zz_MMO_01_pop_assembly_buildings.txt", "isFromGame": false },
      "definitions": [{ "mod": "...", "file": "...", "isFromGame": false }]
    }
  ]
}
```

`definitions` lists the definitions in Irony's evaluated order. `unmatched` lists playlist or collection entries with no installed mod behind them.

## Building

```
dotnet build src/IronyModManager.CLI/IronyModManager.CLI.csproj
dotnet test src/IronyModManager.CLI.Tests/IronyModManager.CLI.Tests.csproj
```

The implementation projects (IO, Models, Parser, Services, Storage) are ordinary project references, so their assemblies and NuGet dependencies land in the output folder on every platform. `References/CopyAll` (parser maps) and the GUI's `Localization/*.json` are copied next to the executable.

### Linux

The repository's build expects two things that only exist on Windows build machines:

- **Strong-name keys** `keys/Irony-*.snk`. Generate local ones, for example with Mono's `sn -k`, and copy the public keys into `src/IronyModManager.DI/` as described in the main README. Never commit them.
- **The pre-build step** `Tools/LocalizationResourceGenerator/LocalizationResourceGenerator.exe`. On Linux, replace it with a shell script that runs the framework-dependent `LocalizationResourceGenerator.dll` through `dotnet`. The regenerated `src/IronyModManager.Shared/LocalizationResources.cs` then differs only in line endings, so don't commit it.
