using System;
using Dalamud.Interface.Windowing;
using System.Numerics;
using ImGuiNET;

namespace SamplePlugin.Windows;

public class MainWindow : Window, IDisposable
{

    private readonly Plugin plugin;
    
    //  Constructor
    public MainWindow(Plugin plugin)
        : base("BaseWindow", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        

        this.plugin = plugin;
    }

    //  Dispose
    public void Dispose() { }

    // Pre-Render
    public override void PreDraw()
    {
        
    }
    
    //  Render test
    public override void Draw()
    {

        if (ImGui.Button("Puzzle Panel##"))
        {
            plugin.TogglePPUI();
        }
    }
}
