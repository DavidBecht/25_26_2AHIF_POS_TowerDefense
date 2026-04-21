using System.Windows;

namespace TowerDefenseGame.Helpers;

public static class PathDefinition
{
    public const int CellSize = 40;
    public const int Columns  = 20;
    public const int Rows     = 12;

    // Wegpunkte in Pixel-Koordinaten.
    // Der Pfad führt von links nach rechts durch den Canvas (800 x 480 px).
    // Einstieg off-screen links, Ausstieg off-screen rechts.
    public static readonly List<Point> Waypoints = new()
    {
        new Point(-40,  100),   // Eintritt (links, off-screen)
        new Point(220,  100),   // Ecke oben links
        new Point(220,  260),   // runter
        new Point(460,  260),   // rechts
        new Point(460,  100),   // hoch
        new Point(660,  100),   // rechts oben
        new Point(660,  380),   // runter
        new Point(840,  380),   // Austritt (rechts, off-screen)
    };
}
