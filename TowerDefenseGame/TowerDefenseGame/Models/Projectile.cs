using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TowerDefenseGame.Models;

public class Projectile
{
    public Point  Position  { get; private set; }
    public Vector Direction { get; }   // normalisierter Einheitsvektor
    public double Speed     { get; }
    public int    Damage    { get; }
    public bool   IsActive  { get; private set; } = true;

    private Ellipse? _shape;

    public Projectile(Point startPosition, Vector direction, double speed, int damage)
    {
        Position  = startPosition;
        Direction = direction;
        Speed     = speed;
        Damage    = damage;
    }

    // -------------------------------------------------------------------------
    // WP6: Projektil bewegen
    // -------------------------------------------------------------------------

    public void Move(double deltaTime)
    {
        // TODO (WP6): Bewege das Projektil.
        //
        // Neue Position:
        //   double newX = Position.X + Direction.X * Speed * deltaTime;
        //   double newY = Position.Y + Direction.Y * Speed * deltaTime;
        //   Position = new Point(newX, newY);
        //
        // Setze IsActive = false wenn das Projektil außerhalb des Canvas ist:
        //   x < -50 || x > 850 || y < -50 || y > 530
    }

    // -------------------------------------------------------------------------
    // WP7: Kollision mit Gegnern prüfen
    // -------------------------------------------------------------------------

    public void CheckCollision(List<Enemy> enemies)
    {
        // TODO (WP7): Prüfe ob dieses Projektil einen lebenden Gegner trifft.
        //
        // Für jeden Gegner in enemies (nur IsAlive == true):
        //   - Berechne Distanz zwischen Position und enemy.Position
        //   - Wenn Distanz < 18 (Treffradius):
        //       enemy.TakeDamage(Damage)
        //       IsActive = false
        //       return  ← Projektil kann nur einmal treffen
    }

    // -------------------------------------------------------------------------
    // Zeichnen (fertig implementiert)
    // -------------------------------------------------------------------------

    public void Draw(Canvas canvas)
    {
        if (_shape == null)
        {
            _shape = new Ellipse
            {
                Width  = 8,
                Height = 8,
                Fill   = Brushes.Yellow,
                Stroke = Brushes.OrangeRed,
                StrokeThickness = 1,
            };
            canvas.Children.Add(_shape);
        }
        Canvas.SetLeft(_shape, Position.X - 4);
        Canvas.SetTop(_shape,  Position.Y - 4);
    }

    public void RemoveFromCanvas(Canvas canvas)
    {
        if (_shape != null) canvas.Children.Remove(_shape);
        _shape = null;
    }
}
