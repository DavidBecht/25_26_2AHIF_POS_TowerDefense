namespace TowerDefenseGame.Models;

public class WaveSpawner
{
    public int  CurrentWave { get; private set; } = 0;
    public bool IsSpawning  { get; private set; } = false;

    private int    _enemiesToSpawn = 0;
    private double _spawnTimer     = 0;
    private double _spawnInterval  = 1.2; // Sekunden zwischen zwei Gegnern

    // -------------------------------------------------------------------------
    // WP8: Nächste Welle starten
    // -------------------------------------------------------------------------
    
    public void StartNextWave()
    {
        // TODO (WP8): Bereite die nächste Welle vor.
        //
        // 1. CurrentWave++ (erhöhe die Wellenzahl)
        this.CurrentWave++;

        // 2. Berechne Anzahl der Gegner: _enemiesToSpawn = 5 + CurrentWave * 2
        this._enemiesToSpawn = 5 + CurrentWave * 2;

        // 3. Verringere das Spawn-Intervall leicht mit jeder Welle (Minimum 0.4s):
        //       _spawnInterval = Math.Max(0.4, 1.2 - CurrentWave * 0.05)
        this._spawnInterval = Math.Max(0.4, 1.2 - CurrentWave * 0.05);

        // 4. Setze IsSpawning = true und _spawnTimer = 0
        this.IsSpawning = true;
        this._spawnTimer = 0;
    }

    // -------------------------------------------------------------------------
    // WP9: Spawner-Update – gibt einen neuen Gegner zurück oder null
    // -------------------------------------------------------------------------

    public Enemy? Update(double deltaTime)
    {
        // TODO (WP9): Spawne Gegner im Takt von _spawnInterval.
        //
        // 1. Wenn IsSpawning == false oder _enemiesToSpawn <= 0: return null
        // 2. Reduziere _spawnTimer um deltaTime
        // 3. Wenn _spawnTimer > 0: return null (noch nicht Zeit)
        // 4. Setze _spawnTimer = _spawnInterval zurück
        // 5. Reduziere _enemiesToSpawn um 1
        // 6. Wenn _enemiesToSpawn == 0: IsSpawning = false
        // 7. Erzeuge und gib einen Gegner zurück:
        //    - Jede 3. Welle: return new TankEnemy()
        //    - Jede 2. Welle: return new FastEnemy()
        //    - Sonst:         return new Enemy(hp: 80 + CurrentWave * 15, speed: 1.2, reward: 10)
        //
        // Tipp: Verwende CurrentWave % 3 == 0 bzw. CurrentWave % 2 == 0 für die Bedingungen.
        return null;
    }
}
