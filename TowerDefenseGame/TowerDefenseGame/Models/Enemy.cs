using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using TowerDefenseGame.Helpers;

namespace TowerDefenseGame.Models;

public class Enemy
{
    public Point Position   { get; protected set; }
    public int   MaxHp      { get; }
    public int   Hp         { get; private set; }
    public double Speed     { get; }
    public bool  IsAlive    { get; protected set; } = true;
    public bool  ReachedEnd { get; private set; }   = false;
    public int   Reward     { get; }

    // Index des nächsten anzusteuernden Wegpunkts
    private int _waypointIndex = 1;

    private Ellipse?   _body;
    private Rectangle? _hpBarBg;
    private Rectangle? _hpBar;

    public Enemy(int hp, double speed, int reward)
    {
        MaxHp    = hp;
        Hp       = hp;
        Speed    = speed;
        Reward   = reward;
        Position = PathDefinition.Waypoints[0];
    }

    // -------------------------------------------------------------------------
    // WP1: Gegner entlang des Pfades bewegen
    // -------------------------------------------------------------------------

    public virtual void Move()
    {
        // TODO (WP1): Bewege den Gegner in Richtung PathDefinition.Waypoints[_waypointIndex].
        //
        // Algorithmus:
        // 1. Berechne den Vektor vom aktuellen Position zum Ziel-Wegpunkt:
        //       double dx = ziel.X - Position.X;
        //       double dy = ziel.Y - Position.Y;
        // 2. Berechne die Distanz: Math.Sqrt(dx*dx + dy*dy)
        // 3. Wenn Distanz < Speed (Gegner ist nah genug am Wegpunkt):
        //       _waypointIndex++
        //       Wenn _waypointIndex >= Waypoints.Count:
        //           ReachedEnd = true; IsAlive = false; return;
        // 4. Normalisiere den Vektor (teile durch Distanz) und multipliziere mit Speed.
        // 5. Setze Position = new Point(Position.X + dx, Position.Y + dy)
    }

    // -------------------------------------------------------------------------
    // WP2: Schaden nehmen
    // -------------------------------------------------------------------------

    public void TakeDamage(int damage)
    {
        // TODO (WP2): Ziehe damage von Hp ab.
        // - Stelle sicher dass Hp nicht unter 0 fällt (Math.Max).
        // - Wenn Hp == 0: IsAlive = false setzen.
    }

    // -------------------------------------------------------------------------
    // Zeichnen (fertig implementiert)
    // -------------------------------------------------------------------------

    public void Draw(Canvas canvas)
    {
        if (_body == null)
        {
            _hpBarBg = new Rectangle { Width = 28, Height = 5, Fill = Brushes.DarkRed };
            _hpBar   = new Rectangle { Width = 28, Height = 5, Fill = Brushes.LimeGreen };
            _body    = new Ellipse
            {
                Width  = GetSize(),
                Height = GetSize(),
                Fill   = GetColor(),
                Stroke = Brushes.Black,
                StrokeThickness = 1.5,
            };
            canvas.Children.Add(_hpBarBg);
            canvas.Children.Add(_hpBar);
            canvas.Children.Add(_body);
        }

        double half = GetSize() / 2.0;
        Canvas.SetLeft(_body,   Position.X - half);
        Canvas.SetTop(_body,    Position.Y - half);

        double fraction = (double)Hp / MaxHp;
        _hpBar!.Width = 28 * fraction;
        Canvas.SetLeft(_hpBarBg, Position.X - 14);
        Canvas.SetTop(_hpBarBg,  Position.Y - half - 8);
        Canvas.SetLeft(_hpBar,   Position.X - 14);
        Canvas.SetTop(_hpBar,    Position.Y - half - 8);
    }

    public void RemoveFromCanvas(Canvas canvas)
    {
        if (_body   != null) canvas.Children.Remove(_body);
        if (_hpBar  != null) canvas.Children.Remove(_hpBar);
        if (_hpBarBg != null) canvas.Children.Remove(_hpBarBg);
        _body = null; _hpBar = null; _hpBarBg = null;
    }

    public int getWaypointIdx() => this._waypointIndex;

    protected virtual Brush  GetColor() => Brushes.Crimson;
    protected virtual double GetSize()  => 24;
}
