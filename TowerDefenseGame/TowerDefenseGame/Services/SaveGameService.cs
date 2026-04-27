using System.IO;
using System.Text.Json;
using System.Windows;
using Serilog;
using TowerDefenseGame.Models;

namespace TowerDefenseGame.Services;

public static class SaveGameService
{
    private const string SavePath = "savegame.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
    };

    // -------------------------------------------------------------------------
    // WP20a: Spielstand speichern
    // -------------------------------------------------------------------------

    public static void Save(GameState state)
    {
        // TODO (WP20a): Konvertiere den aktuellen GameState in ein SaveGame-Objekt
        // und serialisiere es als JSON in die Datei SavePath.
        //
        // Schritte:
        // 1. Erstelle ein SaveGame-Objekt:
        //       var save = new SaveGame
        //       {
        //           Lives       = state.Lives,
        //           Score       = state.Score,
        //           CurrentWave = state.WaveSpawner.CurrentWave,
        //       };
        //
        // 2. Fülle save.Towers – für jeden Tower in state.Towers:
        //       save.Towers.Add(new TowerSaveData
        //       {
        //           X    = tower.Position.X,
        //           Y    = tower.Position.Y,
        //           Type = tower.TowerType,    // ← TowerType-Property muss in Tower ergänzt werden (s. Hinweis)
        //       });
        //
        // 3. Serialisiere und schreibe:
        //       string json = JsonSerializer.Serialize(save, JsonOptions);
        //       File.WriteAllText(SavePath, json);
        //
        // 4. Logging:
        Log.Information("Spielstand gespeichert: Welle {Wave}, Score {Score}, Türme {Count}");//Es gibt all diese Variablen noch nicht
        //                        save.CurrentWave, save.Score, save.Towers.Count);
        //
        // Hinweis: Tower braucht eine neue Property:
        //       public TowerType TowerType { get; }
        // Ergänze sie im Konstruktor von Tower (und den Subklassen).
        //
        // Fehlerbehandlung: Wrap alles in try/catch – bei Fehler Log.Error(...) aufrufen.
    }

    // -------------------------------------------------------------------------
    // WP20b: Spielstand laden
    // -------------------------------------------------------------------------

    public static SaveGame? Load()
    {
        // TODO (WP20b): Lese die JSON-Datei und deserialisiere sie zurück in ein SaveGame.
        //
        // Schritte:
        // 1. Prüfe ob die Datei existiert:
               if (!File.Exists(SavePath)) { Log.Warning("Keine Speicherdatei gefunden."); return null; }
        //
        // 2. Lese und deserialisiere:
        //       string json   = File.ReadAllText(SavePath);
        //       var    save   = JsonSerializer.Deserialize<SaveGame>(json, JsonOptions);
        //
        // 3. Logging:
        Log.Information("Spielstand geladen: Welle {Wave}, Score {Score}"); //Es gibt diese Variablen auch noch nicht
        //                        save.CurrentWave, save.Score);
        //
        // 4. Gib save zurück.
        //
        // Fehlerbehandlung: Wrap alles in try/catch – bei Fehler Log.Error(...) aufrufen, null zurückgeben.
        return null;
    }
}
