# Tower Defense – Klassenprojekt POS1

Ein halbfertiges Tower Defense Spiel in **C# / WPF (.NET 8)**.  
Ihr implementiert die Spiellogik gemeinsam – verteilt auf Feature Branches mit Pull Requests.

## Spielprinzip

Gegner laufen entlang eines vorgegebenen Pfades von links nach rechts über den Canvas.  
Platziert Türme neben dem Pfad, die automatisch auf Gegner schießen.  
Erreicht ein Gegner das Ende, verliert ihr ein Leben. Tötet ihr Gegner, steigt der Score.

## Projekt starten

```bash
git clone https://github.com/EUER-KLASSEN-REPO/tower-defense.git
cd tower-defense/TowerDefenseGame
dotnet run --project TowerDefenseGame
```

Oder Solution in Visual Studio öffnen: `TowerDefenseGame/TowerDefenseGame.sln`

**Voraussetzungen:** .NET 8 SDK, Windows (WPF)


## Projektstruktur

```
TowerDefense/
  Angabe/
    00_arbeitspackete.pdf     ← Alle Arbeitsaufträge, Git-Workflow, Minimalset
  TowerDefenseGame/
    TowerDefenseGame.sln
    TowerDefenseGame/
      Helpers/
        PathDefinition.cs    ← Pfad-Wegpunkte und Grid-Konstanten (fertig)
        GridHelper.cs        ← Pfad-Kollisionsprüfung für Turmplatzierung (fertig)
      Models/
        Enemy.cs             ← WP01, WP02
        Tower.cs             ← WP03, WP04, WP05
        Projectile.cs        ← WP06, WP07
        WaveSpawner.cs       ← WP08, WP09
        GameState.cs         ← WP10, WP11
        FastEnemy.cs         ← WP16
        TankEnemy.cs         ← WP17
        SniperTower.cs       ← WP18
        MachineGunTower.cs   ← WP19
        SaveGame.cs          ← Datenklasse für Spielstand (fertig)
      Services/
        SaveGameService.cs   ← WP20 (Speichern/Laden)
      MainWindow.xaml        ← UI-Layout (fertig)
      MainWindow.xaml.cs     ← WP12, WP13, WP14, WP15
      App.xaml.cs            ← WP21 (Logging)
```

## Git-Workflow

```bash
# 1. Aktuellen Stand holen
git pull

# 2. Feature Branch erstellen
git checkout -b feature/wp-01-enemy-move

# 3. Implementieren und committen
git add .
git commit -m "WP01: Enemy.Move() implementiert"

# 4. Branch pushen
git push -u origin feature/wp-01-enemy-move

# 5. Pull Request auf GitHub öffnen → Ziel: main
#    Ein anderes Team reviewed und approved den PR
#    Merge in main
```

**Branch-Schema:** `feature/wp-XX-kurzbeschreibung`

## Minimalset – 12 Pakete für ein spielbares Spiel

| # | Paket | Datei |
|---|---|---|
| 1 | WP13 – UI Labels | `MainWindow.xaml.cs` |
| 2 | WP02 – TakeDamage | `Models/Enemy.cs` |
| 3 | WP08 – StartNextWave | `Models/WaveSpawner.cs` |
| 4 | WP09 – WaveSpawner.Update | `Models/WaveSpawner.cs` |
| 5 | WP01 – Enemy.Move | `Models/Enemy.cs` |
| 6 | WP04 – CanShoot | `Models/Tower.cs` |
| 7 | WP03 – FindTarget | `Models/Tower.cs` |
| 8 | WP05 – Tower.Shoot | `Models/Tower.cs` |
| 9 | WP06 – Projectile.Move | `Models/Projectile.cs` |
| 10 | WP07 – CheckCollision | `Models/Projectile.cs` |
| 11 | WP10 – TryPlaceTower | `Models/GameState.cs` |
| 12 | WP11 – GameState.Update | `Models/GameState.cs` |

Alle Details, Beschreibungen und Zusatzpakete: [`Angabe/00_arbeitspackete.pdf`](Angabe/00_arbeitspackete.pdf)
