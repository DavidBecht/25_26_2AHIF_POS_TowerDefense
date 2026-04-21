using TowerDefenseGame.Models;

namespace TowerDefenseGame.Models;

// Speicherstand – wird als JSON serialisiert/deserialisiert.
// Gegner und Projektile werden NICHT gespeichert (flüchtige Spielobjekte).
public class SaveGame
{
    public int  Lives       { get; set; }
    public int  Score       { get; set; }
    public int  CurrentWave { get; set; }
    public List<TowerSaveData> Towers { get; set; } = new();
}

// Minimale Turmdaten die für die Rekonstruktion benötigt werden
public class TowerSaveData
{
    public double     X    { get; set; }
    public double     Y    { get; set; }
    public TowerType  Type { get; set; }
}
