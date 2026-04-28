using System.Windows;
using System.Windows.Media;

namespace TowerDefenseGame.Models;

// WP19: Maschinengewehr-Turm – kleine Reichweite, hoher Schaden, sehr schnelle Schussrate
public class MachineGunTower : Tower
{
    // TODO (WP19): Erstelle den MachineGunTower.
    //
    // Konstruktor-Aufruf (bereits vorgegeben):
    //   base(position, range: 100, damage: 40, fireRate: 4.0)
    //
    // Aufgaben:
    // 1. Überschreibe GetColor() → Brushes.OrangeRed (oder eigene Wahl)
    //
    // Optional (Zusatz):
    // 2. Der MachineGunTower soll nach 8 Schüssen eine kurze Überhitzungs-Pause einlegen
    //    (z. B. 1.5 Sekunden keine Schüsse).
    //    Tipp: Zähle Schüsse in einem privaten Feld _shotCount; überschreibe Shoot().

    public MachineGunTower(Point position) : base(position, range: 100, damage: 40, fireRate: 4.0, towerType: TowerType.MachineGun) { }

    protected override Brush GetColor() => Brushes.OrangeRed; // TODO (WP19): Farbe wählen

    
}
