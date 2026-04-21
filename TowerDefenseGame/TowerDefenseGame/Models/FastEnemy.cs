using System.Windows.Media;

namespace TowerDefenseGame.Models;

// WP16: Schneller, schwacher Gegner
public class FastEnemy : Enemy
{
    // TODO (WP16): Erstelle einen schnellen, schwachen Gegner.
    //
    // Konstruktor-Aufruf (bereits vorgegeben):
    //   base(hp: 40, speed: 3.5, reward: 5)
    //
    // Aufgaben:
    // 1. Überschreibe GetColor() → return Brushes.Orange;
    // 2. Überschreibe GetSize()  → return 18;  (kleiner als normaler Gegner)
    //
    // Optional (Zusatz):
    // 3. Überschreibe Move() um eine leicht geschwungene Bewegung einzubauen
    //    (kleiner Sinus-Offset senkrecht zur Bewegungsrichtung).

    public FastEnemy() : base(hp: 40, speed: 3.5, reward: 5) { }

    protected override Brush GetColor() => Brushes.Orange;  // TODO (WP16): Farbe wählen
    protected override double GetSize() => 18;              // TODO (WP16): Größe anpassen
}
