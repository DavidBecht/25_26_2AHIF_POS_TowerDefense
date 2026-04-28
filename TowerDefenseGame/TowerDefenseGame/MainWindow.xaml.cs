using System.Diagnostics.Eventing.Reader;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Serilog;
using Serilog.Core;
using TowerDefenseGame.Helpers;
using TowerDefenseGame.Models;
using TowerDefenseGame.Services;

namespace TowerDefenseGame;

public partial class MainWindow : Window
{
    private GameState _state = new();
    private readonly DispatcherTimer _timer = new();
    private DateTime _lastTick = DateTime.Now;

    // WP12: Kreis zur Range-Visualisierung
    private Ellipse? _rangeCircle;

    public MainWindow()
    {
        InitializeComponent();
        DrawGrid();
        DrawPath();
        InitTimer();
        UpdateUI();
        Log.Information("Hauptfenster geladen – Spiel bereit.");
        Log.Information("Hauptfenster geladen – Spiel bereit.");
    }

    // -------------------------------------------------------------------------
    // Game Loop
    // -------------------------------------------------------------------------

    private void InitTimer()
    {
        _timer.Interval = TimeSpan.FromMilliseconds(16); // ~60 FPS
        _timer.Tick += GameLoop;
        _timer.Start();
    }

    private void GameLoop(object? sender, EventArgs e)
    {
        var now = DateTime.Now;
        double dt = (now - _lastTick).TotalSeconds;
        _lastTick = now;

        if (_state.IsGameOver) return;

        // Neue Gegner spawnen (WP8 / WP9)
        var newEnemy = _state.WaveSpawner.Update(dt);
        if (newEnemy != null)
            _state.Enemies.Add(newEnemy);

        // Gegner bewegen (WP1)
        foreach (var enemy in _state.Enemies)
            enemy.Move();

        // Türme: Ziel suchen und schießen (WP3 / WP4 / WP5)
        var newProjectiles = new List<Projectile>();
        foreach (var tower in _state.Towers)
        {
            if (tower.CanShoot(dt))
            {
                var target = tower.FindTarget(_state.Enemies);
                if (target != null)
                {
                    var p = tower.Shoot(target);
                    if (p != null) newProjectiles.Add(p);
                }
            }
        }
        _state.Projectiles.AddRange(newProjectiles);

        // Projektile bewegen + Kollision prüfen (WP6 / WP7)
        foreach (var p in _state.Projectiles)
        {
            p.Move(dt);
            p.CheckCollision(_state.Enemies);
        }

        // Spielzustand bereinigen (WP11)
        _state.Update(GameCanvas);

        // Alles zeichnen
        foreach (var enemy in _state.Enemies)    enemy.Draw(GameCanvas);
        foreach (var tower in _state.Towers)     tower.Draw(GameCanvas);
        foreach (var proj  in _state.Projectiles) proj.Draw(GameCanvas);

        UpdateUI();

        if (_state.IsGameOver)
            ShowGameOver();
    }

    // -------------------------------------------------------------------------
    // WP13: UI-Labels aktualisieren
    // -------------------------------------------------------------------------

    private void UpdateUI()
    {
        TxtLives.Text  = $"❤  Leben:  {_state.Lives}";
        TxtScore.Text  = $"★  Score:  {_state.Score}";
        TxtWave.Text   = $"〜  Wave:  {_state.WaveSpawner.CurrentWave}";
        BtnNextWave.IsEnabled = !_state.WaveSpawner.IsSpawning;
    }

    // -------------------------------------------------------------------------
    // WP14: Game-Over-Overlay anzeigen
    // -------------------------------------------------------------------------

    private void ShowGameOver()
    {
        Rectangle rect = new Rectangle
        {
            Width = 800,
            Height = 480,
            Fill = new SolidColorBrush(Color.FromArgb(160, 0, 0, 0))
        };
        GameCanvas.Children.Add(rect);

        TextBlock gameOverText = new TextBlock
        {
            Text = "GAME OVER",
            FontSize = 48,
            Foreground = Brushes.Red,
            FontWeight = FontWeights.Bold
        };


        Canvas.SetLeft(gameOverText, (800 - 300) / 2);
        Canvas.SetTop(gameOverText, 150);
        GameCanvas.Children.Add(gameOverText);


        TextBlock scoreText = new TextBlock
        {
            FontSize = 32,
            Foreground = Brushes.White,
            Text = Convert.ToString(TxtScore.Text)
        };

        Canvas.SetLeft(scoreText, (800 - 200) / 2);
        Canvas.SetTop(scoreText, 230);
        GameCanvas.Children.Add(scoreText);

        _timer.Stop();


    }

    // -------------------------------------------------------------------------
    // WP12: Reichweiten-Kreis beim Hover über Turm anzeigen
    // -------------------------------------------------------------------------

    private void GameCanvas_MouseMove(object sender, MouseEventArgs e)
    {
        // TODO (WP12): Zeige einen halbtransparenten Kreis um den Turm, über dem
        // die Maus schwebt.
        //
        // Schritte:
        // 1. Bestimme Mausposition: var pos = e.GetPosition(GameCanvas);
        // 2. Suche in _state.Towers den Tower, dessen Position < 20px von pos entfernt ist.
        // 3. Wenn gefunden (und anders als zuvor):
        //    - Entferne alten _rangeCircle aus Canvas (wenn vorhanden).
        //    - Erstelle neuen Ellipse mit Width = Height = tower.Range * 2,
        //      Fill = halbtransparent weiß (Opacity ~0.15), Stroke = weiß.
        //    - Positioniere mit Canvas.SetLeft(tower.Position.X - tower.Range) usw.
        //    - Füge zum Canvas hinzu, speichere in _rangeCircle.
        // 4. Wenn kein Turm in der Nähe: _rangeCircle entfernen und auf null setzen.

        Point mousePosition = e.GetPosition(GameCanvas);

        foreach (Tower t in _state.Towers)
        {
            Point towerPosition = t.Position;

            if (Math.Abs(mousePosition.X - towerPosition.X) < 20 && Math.Abs(mousePosition.Y - towerPosition.Y) < 20)
            {
                GameCanvas.Children.Remove(_rangeCircle);

                Ellipse rangeCircle = new Ellipse();
                rangeCircle.Width = t.Range * 2;
                rangeCircle.Height = t.Range * 2;
                rangeCircle.Fill = new SolidColorBrush(Color.FromArgb(40, 255, 255, 255));
                rangeCircle.Stroke = Brushes.White;
                rangeCircle.StrokeThickness = 1;

                Canvas.SetLeft(rangeCircle, towerPosition.X - t.Range);
                Canvas.SetTop(rangeCircle, towerPosition.Y - t.Range);

                GameCanvas.Children.Add(rangeCircle);

                _rangeCircle = rangeCircle; 
                return;
            }
        }
        GameCanvas.Children.Remove(_rangeCircle);
        _rangeCircle = null;



    }

    // -------------------------------------------------------------------------
    // Eingabe
    // -------------------------------------------------------------------------

    private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (_state.IsGameOver) return;
        var pos = e.GetPosition(GameCanvas);
        _state.TryPlaceTower(pos, GetSelectedTowerType());
    }

    private TowerType GetSelectedTowerType()
    {
        if (RbSniper.IsChecked     == true) return TowerType.Sniper;
        if (RbMachineGun.IsChecked == true) return TowerType.MachineGun;
        return TowerType.Basic;
    }

    private void BtnNextWave_Click(object sender, RoutedEventArgs e)
    {
        if (!_state.WaveSpawner.IsSpawning)
        {
            _state.WaveSpawner.StartNextWave();
            Log.Information("Welle {Wave} gestartet.", _state.WaveSpawner.CurrentWave);
            Log.Information("Welle {Wave} gestartet.", _state.WaveSpawner.CurrentWave); // Es gibt Wave nicht

        }
    }

    private void BtnRestart_Click(object sender, RoutedEventArgs e)
    {
        Log.Information("Spiel neu gestartet.");
        _state = new GameState();
        _rangeCircle = null;
        GameCanvas.Children.Clear();
        DrawGrid();
        DrawPath();
        _lastTick = DateTime.Now;
        _timer.Start();
        UpdateUI();
        Log.Information("Spiel neu gestartet.");
    }

    // -------------------------------------------------------------------------
    // WP20: Speichern / Laden
    // -------------------------------------------------------------------------

    private void BtnSave_Click(object sender, RoutedEventArgs e)
    {
        SaveGameService.Save(_state);
        MessageBox.Show("Spielstand gespeichert!", "Gespeichert", MessageBoxButton.OK, MessageBoxImage.Information);
        // TODO (WP20a): Spielstand speichern.
        //
        // 1. Rufe SaveGameService.Save(_state) auf.
        // 2. Zeige eine kurze Bestätigung in der UI:
        //       MessageBox.Show("Spielstand gespeichert!", "Gespeichert",
        //                        MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void BtnLoad_Click(object sender, RoutedEventArgs e)
    {
        SaveGame? save = SaveGameService.Load();
        if (save == null)
        {
            MessageBox.Show("Spiel konnte gespeichert werden!");
            return;
        }
        _state = new GameState();
        GameCanvas.Children.Clear();
        DrawGrid(); DrawPath();
        _state.LoadFrom(save);
        foreach (var td in save.Towers)
            _state.TryPlaceTower(new Point(td.X, td.Y), td.Type);
        UpdateUI();
        _lastTick = DateTime.Now;
        _timer.Start();
        // TODO (WP20b): Spielstand laden und Spielfeld wiederherstellen.
        //
        // 1. Rufe SaveGame? save = SaveGameService.Load() auf.
        //    Wenn save == null: MessageBox mit Fehlermeldung, return.
        //
        // 2. Spielfeld zurücksetzen (wie BtnRestart):
        //       _state = new GameState();
        //       GameCanvas.Children.Clear();
        //       DrawGrid(); DrawPath();
        //
        // 3. Lives, Score und CurrentWave aus save wiederherstellen.
        //    Hinweis: GameState braucht dafür Setter oder eine Methode LoadFrom(SaveGame).
        //    Ergänze in GameState: public void LoadFrom(SaveGame save) { ... }
        //
        // 4. Türme neu platzieren:
        //       foreach (var td in save.Towers)
        //           _state.TryPlaceTower(new Point(td.X, td.Y), td.Type);
        //
        // 5. UI aktualisieren und Timer starten.
        //       UpdateUI();
        //       _lastTick = DateTime.Now;
        //       _timer.Start();
    }

    // -------------------------------------------------------------------------
    // Hintergrund: Grid und Pfad zeichnen (fertig implementiert)
    // -------------------------------------------------------------------------

    private void DrawGrid()
    {
        int cs = PathDefinition.CellSize;
        var gridColor = new SolidColorBrush(Color.FromArgb(35, 255, 255, 255));

        for (int col = 0; col <= PathDefinition.Columns; col++)
        {
            GameCanvas.Children.Add(new Line
            {
                X1 = col * cs, Y1 = 0,
                X2 = col * cs, Y2 = PathDefinition.Rows * cs,
                Stroke = gridColor, StrokeThickness = 0.5
            });
        }
        for (int row = 0; row <= PathDefinition.Rows; row++)
        {
            GameCanvas.Children.Add(new Line
            {
                X1 = 0,                      Y1 = row * cs,
                X2 = PathDefinition.Columns * cs, Y2 = row * cs,
                Stroke = gridColor, StrokeThickness = 0.5
            });
        }
    }

    private void DrawPath()
    {
        var wp = PathDefinition.Waypoints;
        for (int i = 0; i < wp.Count - 1; i++)
        {
            var line = new Line
            {
                X1 = wp[i].X,   Y1 = wp[i].Y,
                X2 = wp[i+1].X, Y2 = wp[i+1].Y,
                Stroke = new SolidColorBrush(Color.FromRgb(180, 140, 70)),
                StrokeThickness = PathDefinition.CellSize - 6,
                StrokeStartLineCap = PenLineCap.Round,
                StrokeEndLineCap   = PenLineCap.Round,
            };
            Panel.SetZIndex(line, -1);
            GameCanvas.Children.Add(line);
        }
    }

    private void DisplayStats(double range, double damage, double firerate)
    {
        TxtTowerRange.Text = $"Reichweite:     {range} px";
        TxtTowerDamage.Text = $"Schaden:        {damage} ";
        TxtTowerFireRate.Text = $"Schussrate:     {firerate} / s";
    }

    private void RbBasic_Checked(object sender, RoutedEventArgs e)
    {
        DisplayStats(120, 20, 1);
    }

    private void RbSniper_Checked(object sender, RoutedEventArgs e)
    {
        DisplayStats(250, 15, 0.5);
    }

    private void RbMachineGun_Checked(object sender, RoutedEventArgs e)
    {
        DisplayStats(100, 40, 4);
    }
}
