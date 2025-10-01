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
   
##🚀 Anleitung: UnbanMenuPlugin auf Replit kompilieren
1. Neues Projekt erstellen

Gehe auf https://replit.com

Klicke oben rechts auf + Create Repl

Wähle als Template: C# (.NET)

Gib dem Projekt einen Namen, z. B. UnbanMenuPlugin

Klicke auf Create Repl

2. Projektdateien hochladen

Lade die Dateien aus meinem ZIP hoch:

UnbanMenuPlugin.csproj

UnbanMenuPlugin.cs

appsettings.json (falls enthalten für Konfiguration)

Ziehe sie einfach per Drag&Drop in den Replit-Dateibrowser (links).

3. Abhängigkeiten hinzufügen

Das Plugin braucht Zugriff auf die CounterStrikeSharp API.
Dafür im Replit-Terminal Folgendes eingeben:

dotnet add package CounterStrikeSharp.API


(Das zieht automatisch die richtige NuGet-Version.)

4. Projekt bauen (kompilieren)

Im Replit-Terminal eingeben:

dotnet build -c Release


👉 Danach findest du die kompilierten DLLs unter:
bin/Release/net8.0/

Die wichtige Datei ist:
UnbanMenuPlugin.dll

5. DLL herunterladen

Gehe in den Replit-Dateibrowser (bin/Release/net8.0/)

Klicke auf die drei Punkte bei UnbanMenuPlugin.dll → Download

6. Installation auf deinem CS2-Server

Kopiere die UnbanMenuPlugin.dll in deinen Serverordner:

/game/csgo/addons/counterstrikesharp/plugins/


(Falls vorhanden) kopiere auch appsettings.json ins selbe Verzeichnis oder configs/ → dort kannst du einstellen, ob nur Root-Admins Zugriff haben.

Server starten → das Plugin sollte automatisch geladen werden.

✨ Tipp: Wenn du die Config ändern willst (z. B. Root-only Zugriff fürs Menü), einfach in der appsettings.json anpassen → kein erneutes Kompilieren nötig.

## Hinweise

- Das Plugin verwendet `MySql.Data` (NuGet) für DB-Zugriff.
- Passe `UnbanMenuPlugin.cs` an, falls deine CSSharp-API andere Klassen-/Methodennamen hat.
- Die config wird automatisch erstellt, ändere die DB-Daten darin.

