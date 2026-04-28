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
    public TowerType TowerType { get; } // Art des Towers
    private double _cooldown = 0;
    private Rectangle? _shape;

    public Tower(Point position, double range, int damage, double fireRate, TowerType towerType)
    {
        Position = position;
        Range    = range;
        Damage   = damage;
        FireRate = fireRate;
        TowerType = towerType;
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
        _cooldown = 1.0 / FireRate;
        Vector direction_vektor_target = target.Position - this.Position;
        direction_vektor_target.Normalize();
        return new Projectile(Position, direction_vektor_target, speed: 300, Damage);
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
