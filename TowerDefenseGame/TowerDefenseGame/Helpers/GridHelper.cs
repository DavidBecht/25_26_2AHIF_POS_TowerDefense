using System.Windows;

namespace TowerDefenseGame.Helpers;

public static class GridHelper
{
    // Gibt true zurück wenn eine Grid-Zelle den Pfad berührt (kein Turm baubar).
    public static bool IsCellOnPath(int col, int row)
    {
        double cx = col * PathDefinition.CellSize + PathDefinition.CellSize / 2.0;
        double cy = row * PathDefinition.CellSize + PathDefinition.CellSize / 2.0;
        var center = new Point(cx, cy);

        var wp = PathDefinition.Waypoints;
        for (int i = 0; i < wp.Count - 1; i++)
        {
            if (DistanceToSegment(center, wp[i], wp[i + 1]) < PathDefinition.CellSize)
                return true;
        }
        return false;
    }

    private static double DistanceToSegment(Point p, Point a, Point b)
    {
        double dx = b.X - a.X;
        double dy = b.Y - a.Y;
        if (dx == 0 && dy == 0) return Distance(p, a);
        double t = Math.Clamp(((p.X - a.X) * dx + (p.Y - a.Y) * dy) / (dx * dx + dy * dy), 0, 1);
        return Distance(p, new Point(a.X + t * dx, a.Y + t * dy));
    }

    private static double Distance(Point a, Point b)
        => Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
}
