# CS2 UnbanMenuPlugin

Dieses Projekt enthält ein kleines CS2/CounterStrikeSharp Plugin, das ein Unban-Menü bereitstellt.
Die Library ist als Ausgangspunkt gedacht — je nach CounterStrikeSharp-Version musst du möglicherweise API-Aufrufe (Commands, MenuFactory, Client) anpassen.

## Quickstart

1. Entpacke das ZIP.
2. Öffne das `src`-Verzeichnis und führe `dotnet build` aus (oder öffne das Projekt in Visual Studio).
3. Kopiere die erzeugte DLL (`bin/Debug/net6.0/...dll`) nach:
   `addons/counterstrikesharp/plugins/UnbanMenuPlugin/`
4. Starte den Server. Beim ersten Start wird `addons/counterstrikesharp/configs/unbanmenu/config.json` erstellt.
5. Als Root-Admin im Spiel `/openunbanmenu` (oder der in der config hinterlegte Command) ausführen.

## Hinweise

- Das Plugin verwendet `MySql.Data` (NuGet) für DB-Zugriff.
- Passe `UnbanMenuPlugin.cs` an, falls deine CSSharp-API andere Klassen-/Methodennamen hat.
- Die config wird automatisch erstellt, ändere die DB-Daten darin.

