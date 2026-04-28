using System.Windows;
using Serilog;

namespace TowerDefenseGame;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // -------------------------------------------------------------------------
        // WP21a: Serilog konfigurieren
        // -------------------------------------------------------------------------

        // TODO (WP21a): Konfiguriere den globalen Serilog-Logger.
        //
        // Beispiel aus dem Skriptum:
        //   Log.Logger = new LoggerConfiguration()
        //       .MinimumLevel.Debug()
        //       .WriteTo.Console()
        //       .WriteTo.File("towerdefense.log",
        //                     rollingInterval: RollingInterval.Day,
        //                     retainedFileCountLimit: 7)
        //       .CreateLogger();
        //
        // Danach:
        //   Log.Information("=== Tower Defense gestartet ===");

        base.OnStartup(e);
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("towerdefense.log", rollingInterval:RollingInterval.Day, retainedFileCountLimit: 7)
            .CreateLogger();
        Log.Information("=== Tower Defense gestartet ===");
        
    }

    protected override void OnExit(ExitEventArgs e)
    {
        // Logger sauber beenden (Puffer leeren)
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}
