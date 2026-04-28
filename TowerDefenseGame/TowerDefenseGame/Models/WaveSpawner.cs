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

    public void SetWave(int wave)
    {
        CurrentWave = wave;
    }
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
        if (IsSpawning == false || _enemiesToSpawn <= 0) return null;
        _spawnTimer -= deltaTime;
        if (_spawnTimer > 0) return null;
        _spawnTimer = _spawnInterval;
        _enemiesToSpawn -= 1;
        if (_enemiesToSpawn == 0) IsSpawning = false;
        if (CurrentWave % 3 == 0) return new TankEnemy();
        else if (CurrentWave % 2 == 0) return new FastEnemy();
        else new Enemy(hp: 80 + CurrentWave * 15, speed: 1.2, reward: 10);
        return null;
    }
}
