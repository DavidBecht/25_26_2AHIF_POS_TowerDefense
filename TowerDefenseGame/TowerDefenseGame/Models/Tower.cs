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

    private static double PointDist(Point point1, Point point2)
    {
        double x_dif_pow = Math.Pow(point1.X - point2.X, 2);
        double y_dif_pow = Math.Pow(point1.Y - point2.Y, 2);
        return Math.Sqrt(x_dif_pow + y_dif_pow);
    }

    public Enemy? FindTarget(List<Enemy> enemies)
    {
        try
        {
            List<Enemy> alive_targets = enemies.Where(x => x.IsAlive).ToList();
            List<Enemy> inrange_targets = alive_targets.Where(x => PointDist(this.Position, x.Position) <= this.Range).ToList();
            Enemy? enemy = inrange_targets.OrderByDescending(x => x.getWaypointIdx()).FirstOrDefault();
            return enemy;
        }
        catch
        {
            return null;
        }
    }

    // -------------------------------------------------------------------------
    // WP4: Schuss-Cooldown verwalten
    // -------------------------------------------------------------------------

    public bool CanShoot(double deltaTime)
    {
        // TODO (WP4): Verwalte den Cooldown-Timer.
        // - Reduziere _cooldown um deltaTime (wenn _cooldown > 0).
        // - Gib true zurück wenn _cooldown <= 0.
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
