using System;
using System.IO;
using Dalamud.Interface.Windowing;
using System.Numerics;
using Dalamud.Interface.Utility;
using ImGuiNET;
using OtterGui;
using ImRaii = OtterGui.Raii.ImRaii;

namespace SamplePlugin.Windows;

public enum EGame
{
    PuzzlePanel,
    MemoryMatch,
    RockPaperScissors
}

public enum EMenuTab
{
    Games,
    Settings,
    HighScores
}

public class Interface : Window, IDisposable
{
    private const float  MinSize    = 700;
    private readonly Plugin plugin;
    private string? menuImgPp;
    private string? menuImgMm;
    private EMenuTab currentTab;
    
    //  Constructor
    public Interface(Plugin plugin)
        : base("MiniGames Menu##", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        

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
                ImGui.SetCursorPos(pos);
                
                ImGui.SetCursorPos(new Vector2(pos.X + 100, pos.Y - 30));
                ImGui.Text("Puzzle Panel");
                
                ImGui.Spacing();
                
                ImGui.SetCursorPos(pos);
                if (ImGui.ImageButton(Services.TextureProvider.GetFromFile(menuImgPp!).GetWrapOrEmpty().ImGuiHandle,
                                      ImGuiHelpers.ScaledVector2(256, 256)))
                {
                    plugin.TogglePPUI();
                }
                break;
            
            case EGame.MemoryMatch:
                ImGui.SetCursorPos(pos);
                
                ImGui.SetCursorPos(new Vector2(pos.X + 100, pos.Y - 30));
                ImGui.Text("Memory Matcher");
                
                ImGui.Spacing();
                
                ImGui.SetCursorPos(pos);
                if (ImGui.ImageButton(Services.TextureProvider.GetFromFile(menuImgMm!).GetWrapOrEmpty().ImGuiHandle,
                                      ImGuiHelpers.ScaledVector2(256, 256)))
                {
                    
                }
                break;
        }
    }


    public void TabHighScores()
    {
        using var id = ImRaii.PushId("HighScores");
        ImRaii.TabItem("HighScores");
        ImGuiUtil.HoverTooltip("High-score for each game.");
    }
    
    public void TabSettings()
    {
        using var id = ImRaii.PushId("Settings");
        ImRaii.TabItem("Settings");
        ImGuiUtil.HoverTooltip("General Settings.\nGame Settings are found in their respective game.");
    }
    
    public void TabGames()
    {
        using var id = ImRaii.PushId("Games");
        ImRaii.TabItem("Games");
        ImGuiUtil.HoverTooltip("MiniGames to play");
    }
    
    
    public void DrawTabs()
    {
        
        ImGui.BeginChild("Tabs");
        ImRaii.TabBar("MainMenuTabs###", ImGuiTabBarFlags.Reorderable);
        TabGames();
        TabSettings();
        TabHighScores();
        ImGui.EndChild();
    }
    
    public void DrawGamesTab()
    {
        ImGui.BeginChild("Game Buttons");
        
        DrawGame(new Vector2(100, 50), EGame.PuzzlePanel);
        DrawGame(new Vector2(365, 50), EGame.MemoryMatch);
        
        ImGui.EndChild();
    }
    
    
    //  Dispose
    public void Dispose() { }

    // Pre-Render
    public override void PreDraw()
    {
        // Skip dalamud size constraints because that would just require unscaling, then scaling.
        var minSize = new Vector2(MinSize,     17 * ImGui.GetTextLineHeightWithSpacing());
        var maxSize = new Vector2(MinSize * 4, ImGui.GetIO().DisplaySize.Y * 15 / 16);
        ImGui.SetNextWindowSizeConstraints(minSize, maxSize);
    }
    
    //  Render test
    public override void Draw()
    {
        
        DrawTabs();
        
        ImGui.Spacing();
        ImGui.Spacing();
    }
}
