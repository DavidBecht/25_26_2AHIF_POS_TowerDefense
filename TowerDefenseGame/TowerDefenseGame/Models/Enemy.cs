using System.Security.Cryptography.Xml;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
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
    private BitmapImage IMG;
     
        

    // Index des nächsten anzusteuernden Wegpunkts
    private int _waypointIndex = 1;

    protected Image? _body;
    private Rectangle? _hpBarBg;
    private Rectangle? _hpBar;

    public Enemy(int hp, double speed, int reward, string img_path = "Assets/enemy1.png")
    {
        MaxHp    = hp;
        Hp       = hp;
        Speed    = speed;
        Reward   = reward;
        Position = PathDefinition.Waypoints[0];
        IMG = new BitmapImage(new Uri(img_path, UriKind.Relative));

    }

    // -------------------------------------------------------------------------
    // WP1: Gegner entlang des Pfades bewegen
    // -------------------------------------------------------------------------

    public virtual void Move()
    {
        double dx = PathDefinition.Waypoints[_waypointIndex].X - Position.X;
        double dy = PathDefinition.Waypoints[_waypointIndex].Y - Position.Y;
        double distance = Math.Sqrt(dx * dx + dy * dy);

        dx = dx / distance * Speed;
        dy = dy / distance * Speed;
        Position = new Point(Position.X + dx, Position.Y + dy);

        if (distance < Speed)
        {
            if(_waypointIndex < PathDefinition.Waypoints.Count())
                _waypointIndex++;
            if (_waypointIndex >= PathDefinition.Waypoints.Count())
            {
                ReachedEnd = true;
                IsAlive = false;
                return;
            }
        }

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
        int hp = Hp;
        hp -= damage;
        Hp = Math.Max(hp, 0);
        if (Hp == 0)
            IsAlive = false;
    }

    // -------------------------------------------------------------------------
    // Zeichnen (fertig implementiert)
    // -------------------------------------------------------------------------

    public virtual void Draw(Canvas canvas)
    {
        if (_body == null)
        {
            _hpBarBg = new Rectangle { Width = 28, Height = 5, Fill = Brushes.DarkRed };
            _hpBar   = new Rectangle { Width = 28, Height = 5, Fill = Brushes.LimeGreen };            
            canvas.Children.Add(_hpBarBg);
            canvas.Children.Add(_hpBar);
            _body = new Image()
            {
                Width = GetSize(),
                Height = GetSize(),
                Source = IMG
            };
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
