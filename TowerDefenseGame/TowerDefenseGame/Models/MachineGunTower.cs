using System.Windows;
using System.Windows.Media;

namespace TowerDefenseGame.Models;

// WP19: Maschinengewehr-Turm – kleine Reichweite, hoher Schaden, sehr schnelle Schussrate
public class MachineGunTower : Tower
{
  
    public MachineGunTower(Point position) : base(position, range: 100, damage: 40, fireRate: 4.0) { }

    protected override Brush GetColor() => Brushes.OrangeRed; // TODO (WP19): Farbe wählen

    
}
