using System.Windows;
using System.Windows.Media;

namespace TowerDefenseGame.Models;

// WP18: Scharfschützen-Turm – große Reichweite, wenig Schaden, langsame Schussrate
public class SniperTower : Tower
{
    // TODO (WP18): Erstelle den SniperTower.
    //
    // Konstruktor-Aufruf (bereits vorgegeben):
    //   base(position, range: 250, damage: 15, fireRate: 0.5)
    //
    // Aufgaben:
    // 1. Überschreibe GetColor() → Brushes.DarkGreen (oder eigene Wahl)
    //
    // Optional (Zusatz):
    // 2. Überschreibe FindTarget() so dass der Sniper immer den Gegner anvisiert,
    //    der dem Ziel am NÄCHSTEN ist (also am weitesten fortgeschritten),
    //    nicht den nächsten zur Turmposition.
    //    Tipp: Vergleiche den _waypointIndex der Gegner (ggf. als Property ergänzen).

    public SniperTower(Point position) : base(position, range: 250, damage: 15, fireRate: 0.5) { }

    protected override Brush GetColor() => Brushes.DarkGreen; // TODO (WP18): Farbe wählen
}
