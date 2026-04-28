using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;
using TowerDefenseGame.Helpers;

namespace TowerDefenseGame.Models;

public enum TowerType { Basic, Sniper, MachineGun, None }

public class GameState
{
    public List<Enemy>      Enemies     { get; } = new();
    public List<Tower>      Towers      { get; } = new();
    public List<Projectile> Projectiles { get; } = new();
    public WaveSpawner      WaveSpawner { get; } = new();

    public int  Lives      { get; private set; } = 20;
    public int  Score      { get; private set; } = 0;
    public bool IsGameOver { get; private set; } = false;

    // Menge der bereits belegten Zellen (col, row) → kein doppeltes Platzieren
    private readonly HashSet<(int, int)> _occupiedCells = new();

    // -------------------------------------------------------------------------
    // WP20b: Spielstand wiederherstellen (wird von BtnLoad aufgerufen)
    // -------------------------------------------------------------------------

    public void LoadFrom(SaveGame save)
    {
        Lives = save.Lives;
        Score = save.Score;
        WaveSpawner.SetWave(save.CurrentWave);
    
        // TODO (WP20b): Stelle Lives, Score und CurrentWave aus save wieder her.
        //
        // Lives und Score sind private set – ergänze hier direkt die Zuweisung
        // (da LoadFrom in derselben Klasse ist, hat es Zugriff auf private Felder):
        //   Lives  = save.Lives;
        //   Score  = save.Score;
        //   WaveSpawner.SetWave(save.CurrentWave);  ← SetWave-Methode in WaveSpawner ergänzen
        //
        // Hinweis: WaveSpawner benötigt eine neue Methode:
        //   public void SetWave(int wave) { CurrentWave = wave; }
    }

    // -------------------------------------------------------------------------
    // WP10: Turm auf Zelle platzieren
    // -------------------------------------------------------------------------

    public bool TryPlaceTower(Point mousePosition, TowerType selectedType)
    {
        // TODO (WP10): Platziere einen Turm auf der angeklickten Grid-Zelle.
        //
        // Schritte:
        // 1. Berechne Zelle:
        int col = (int)(mousePosition.X / PathDefinition.CellSize);
        int row = (int)(mousePosition.Y / PathDefinition.CellSize);

        // 2. Prüfe ob die Zelle gültig ist (sonst return false):
        if (col < 0 || col >= PathDefinition.Columns)
            return false;

        else if (selectedType == TowerType.None)
            return false;

        else if (row < 0 || row >= PathDefinition.Rows)
            return false;

        else if (GridHelper.IsCellOnPath(col, row))
            return false;

        else if (_occupiedCells.Contains((col, row)))
            return false;

        // 3. Berechne den Pixel-Mittelpunkt der Zelle:
        double cx = col * PathDefinition.CellSize + PathDefinition.CellSize / 2.0;
        double cy = row * PathDefinition.CellSize + PathDefinition.CellSize / 2.0;
        var center = new Point(cx, cy);

        // 4. Erstelle den passenden Turm je nach selectedType:
        Tower tower = selectedType switch
        {
            TowerType.Sniper     => new SniperTower(center),
            TowerType.MachineGun => new MachineGunTower(center),
        _                    => new Tower(center, range: 120, damage: 20, fireRate: 1.0, TowerType.Basic),
        };

        // 5. Füge Turm zur Liste hinzu und markiere Zelle als belegt:
        Towers.Add(tower);
        _occupiedCells.Add((col, row));

        // 6. return true
        Log.Information("Turm {T} auf ({C},{R}) platziert.", selectedType, "col, row"); // Diese zwei Variablen sind auch noch nicht vorhanden.
        return false;
    }

    // -------------------------------------------------------------------------
    // WP11: Spielzustand nach jedem Frame bereinigen
    // -------------------------------------------------------------------------

    public void Update(Canvas canvas)
    {
        // Gegner (iteriere über eine Kopie der Liste mit ToList()):

        foreach (var enemy in Enemies.ToList())
        {
            enemy.RemoveFromCanvas(canvas);
            Enemies.Remove(enemy);
            Log.Debug("Gegner besiegt - Reward {R}. Score jetzt {S}.", enemy.Reward, Score);
            if (enemy.ReachedEnd)
            {
                Lives--;
                Log.Warning("Gegner hat das Ziel erreicht! Leben: {L}", Lives);
            }
            else
                Score += enemy.Reward; // Punkte gutschreiben
        }

        // Projektile (analog):
        foreach (var p in Projectiles.ToList())
        {
            if (!p.IsActive)
            {
                p.RemoveFromCanvas(canvas);
                Projectiles.Remove(p);
            }
        }
        
         // Game Over prüfen:
           if (Lives <= 0){
           IsGameOver = true;
           Log.Warning("=== GAME OVER === Score: {S}, Welle: {W}", Score, WaveSpawner.CurrentWave);
           }
    }
}
