using System.Windows.Media;

namespace TowerDefenseGame.Models;

// WP17: Langsamer, extrem zäher Gegner
public class TankEnemy : Enemy
{
    // TODO (WP17): Erstelle einen langsamen, zähen Gegner.
    //
    // Konstruktor-Aufruf (bereits vorgegeben):
    //   base(hp: 350, speed: 0.7, reward: 25)
    //
    // Aufgaben:
    // 1. Überschreibe GetColor() → Brushes.DarkViolet oder eine andere auffällige Farbe
    // 2. Überschreibe GetSize()  → return 34;  (größer als normaler Gegner)
    //
    // Optional (Zusatz):
    // 3. Überschreibe Draw() um zusätzlich einen dicken Rand (Panzer-Optik) zu zeichnen.
    //    Tipp: Rufe zuerst base.Draw(canvas) auf, dann zeichne darüber.

    public TankEnemy() : base(hp: 350, speed: 0.7, reward: 25) { }

    protected override Brush  GetColor() => Brushes.DarkViolet; // TODO (WP17): Farbe wählen
    protected override double GetSize()  => 34;                  // TODO (WP17): Größe anpassen
}
