using System;
using System.IO;
using Dalamud.Interface.Windowing;
using System.Numerics;
using Dalamud.Interface.Utility;
using ImGuiNET;

namespace SamplePlugin.Windows;

public enum EGame
{
    PuzzlePanel,
    MemoryMatch,
    RockPaperScissors
}


public class MainWindow : Window, IDisposable
{

    private readonly Plugin plugin;
    private string menuImgPp;
    private string menuImgMm;
    
    //  Constructor
    public MainWindow(Plugin plugin)
        : base("MiniGames Menu##", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 330),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        

        this.plugin = plugin;
        GetImgPath();

    }

    public void GetImgPath()
    {
        //  Puzzle Panel
        menuImgPp = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!,
                                 "Assets", "Menu", "PuzzlePanelMenu.png");
        //  Memory Matcher
        menuImgMm = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!,
                                 "Assets", "Menu", "MemoryMatchMenu.png");
    }
    
    public void DrawGame(Vector2 pos, EGame game)
    {
        switch (game)
        {
            case EGame.PuzzlePanel:
                ImGui.BeginChild("Puzzle Panel");
                
                ImGui.SetCursorPos(new Vector2(pos.X + 100, pos.Y - 30));
                ImGui.Text("Puzzle Panel");
                
                ImGui.Spacing();
                
                ImGui.SetCursorPos(pos);
                if (ImGui.ImageButton(Services.TextureProvider.GetFromFile(menuImgPp).GetWrapOrEmpty().ImGuiHandle,
                                      ImGuiHelpers.ScaledVector2(256, 256)))
                {
                    plugin.TogglePPUI();
                }
                ImGui.EndChild();
                break;
            
            case EGame.MemoryMatch:
                ImGui.BeginChild("Memory Matcher");
                
                ImGui.SetCursorPos(new Vector2(pos.X + 100, pos.Y - 30));
                ImGui.Text("Memory Matcher");
                
                ImGui.Spacing();
                
                ImGui.SetCursorPos(pos);
                if (ImGui.ImageButton(Services.TextureProvider.GetFromFile(menuImgMm).GetWrapOrEmpty().ImGuiHandle,
                                      ImGuiHelpers.ScaledVector2(256, 256)))
                {
                    plugin.TogglePPUI();
                }
                ImGui.EndChild();
                break;
        }
        
        
        
        
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
        DrawGame(new Vector2(100, 50), EGame.PuzzlePanel);
        ImGui.SameLine();
        DrawGame(new Vector2(366, 50), EGame.MemoryMatch);
    }
}
