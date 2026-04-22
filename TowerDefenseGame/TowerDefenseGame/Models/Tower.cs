using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TowerDefenseGame.Models;

public class Tower
{
    public Point  Position { get; }
    public double Range    { get; }
    public int    Damage   { get; }
    public double FireRate { get; }  // Schüsse pro Sekunde

    private double _cooldown = 0;
    private Rectangle? _shape;

    public Tower(Point position, double range, int damage, double fireRate)
    {
        Position = position;
        Range    = range;
        Damage   = damage;
        FireRate = fireRate;
    }

    // -------------------------------------------------------------------------
    // WP3: Nächsten Gegner in Reichweite finden
    // -------------------------------------------------------------------------

    public Enemy? FindTarget(List<Enemy> enemies)
    {
        // TODO (WP3): Gib den lebenden Gegner zurück, der am nächsten an Position ist
        // UND dessen Abstand <= Range ist. Gibt null zurück wenn kein Gegner in Reichweite.
        //
        // Tipp: Distanz berechnen mit:
        //   double dist = Math.Sqrt(Math.Pow(e.Position.X - Position.X, 2)
        //                         + Math.Pow(e.Position.Y - Position.Y, 2));
        return null;
    }

    // -------------------------------------------------------------------------
    // WP4: Schuss-Cooldown verwalten
    // -------------------------------------------------------------------------

    public bool CanShoot(double deltaTime)
    {
        // TODO (WP4): Verwalte den Cooldown-Timer.
        // - Reduziere _cooldown um deltaTime (wenn _cooldown > 0).
        // - Gib true zurück wenn _cooldown <= 0.
        if(_cooldown > 0)
            _cooldown -= deltaTime;
        if (_cooldown <= 0)
            return true;
        return false;
    }

    // -------------------------------------------------------------------------
    // WP5: Projektil in Richtung Ziel abfeuern
    // -------------------------------------------------------------------------

    public Projectile? Shoot(Enemy target)
    {
        // TODO (WP5): Erstelle ein Projektil Richtung target.Position.
        //
        // Schritte:
        // 1. Setze Cooldown zurück: _cooldown = 1.0 / FireRate
        // 2. Berechne Richtungsvektor (dx, dy) von Position zu target.Position
        // 3. Normalisiere: teile durch Distanz → ergibt Einheitsvektor
        // 4. Erstelle new Projectile(Position, new Vector(ndx, ndy), speed: 300, Damage)
        // 5. Gib das Projektil zurück
        return null;
    }

    // -------------------------------------------------------------------------
    // Zeichnen (fertig implementiert)
    // -------------------------------------------------------------------------

    public void Draw(Canvas canvas)
    {
        if (_shape == null)
        {
            _shape = new Rectangle
            {
                Width  = 28,
                Height = 28,
                Fill   = GetColor(),
                Stroke = Brushes.Black,
                StrokeThickness = 1.5,
            };
            canvas.Children.Add(_shape);
        }
        Canvas.SetLeft(_shape, Position.X - 14);
        Canvas.SetTop(_shape,  Position.Y - 14);
    }

    public void RemoveFromCanvas(Canvas canvas)
    {
        if (_shape != null) canvas.Children.Remove(_shape);
        _shape = null;
    }

    protected virtual Brush GetColor() => Brushes.SteelBlue;
}
