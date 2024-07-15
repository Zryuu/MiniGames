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

public class Interface : Window, IDisposable
{
    private const string PluginName      = "MiniGames";
    private const float  MinSize         = 700f;
    private const float MenuImgPaddingX  = 20f;
    private const float MenuImgPaddingY  = 10f;
    private const float MenuImgSize      = 256f;
    
    private readonly Plugin plugin;
    private string? menuImgPp;
    private string? menuImgMm;

    //  Constructor
    public Interface(Plugin plugin)
        : base(PluginName +" Menu##", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.plugin = plugin;
        GetMenuImgPath();
    }

    public void GetMenuImgPath()
    {
        //  Puzzle Panel
        menuImgPp = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!,
                                 "Assets", "Menu", "PuzzlePanelMenu.png");
        //  Memory Matcher
        menuImgMm = Path.Combine(Services.PluginInterface.AssemblyLocation.Directory?.FullName!,
                                 "Assets", "Menu", "MemoryMatchMenu.png");
    }
    
    public void DrawGame(Vector2 pos, EGame game, string displayText)
    {
        // Calculate Text size and position
        var textSize = ImGui.CalcTextSize(displayText);
        var textPos = new Vector2(pos.X + ((ImGuiHelpers.ScaledVector2(
                                  256, 256).X - textSize.X) / 2),
                                  pos.Y - textSize.Y - 10);
        
        switch (game)
        {
            case EGame.PuzzlePanel:
                ImGui.SetCursorPos(textPos);
                ImGui.Text(displayText);
                
                ImGui.SetCursorPos(pos);
                if (ImGui.ImageButton(Services.TextureProvider.GetFromFile(menuImgPp!).GetWrapOrEmpty().ImGuiHandle,
                                      ImGuiHelpers.ScaledVector2(MenuImgSize, MenuImgSize)))
                {
                    plugin.TogglePPUI();
                }

                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Match the bottom board to the top.\n More than one will flip.");
                }
                break;
            
            case EGame.MemoryMatch:
                ImGui.SetCursorPos(textPos);
                ImGui.Text(displayText);
                
                ImGui.SetCursorPos(pos);
                if (ImGui.ImageButton(Services.TextureProvider.GetFromFile(menuImgMm!).GetWrapOrEmpty().ImGuiHandle,
                                      ImGuiHelpers.ScaledVector2(MenuImgSize, MenuImgSize)))
                {
                    
                }
                if (ImGui.IsItemHovered())
                {
                    ImGui.SetTooltip("Remember and Match the sequence.");
                }
                
                break;
        }
    }

    public void GetGameScores()
    {
        foreach (EGame game in Enum.GetValues(typeof(EGame)))
        {
            var scores = plugin.HighScoreManager.GetHighScores(game);
            ImGui.Text(game.ToString());

            if (scores.Count == 0)
            {
                ImGui.Text("No high scores yet.");
            }
            else
            {
                foreach (var score in scores)
                {
                    ImGui.Text($"{score.PlayerName}: {score.Score}");
                }
            }

            ImGui.Spacing();
        }
    }
    
    public void TabHighScores()
    {
        ImGui.BeginChild("High Scores");
        
        using var id = ImRaii.PushId("HighScores");
        ImGuiUtil.HoverTooltip("High-score for each game.");

        GetGameScores();

        ImGui.EndChild();
    }
    
    public void TabSettings()
    {
        using var id = ImRaii.PushId("Settings");
        ImGuiUtil.HoverTooltip("General Settings.\nGame Settings are found in their respective game.");
    }
    
    public void TabGames()
    {
        using var id = ImRaii.PushId("Games");
        ImGuiUtil.HoverTooltip("MiniGames to play");
        DrawGamesTab();
    }
    
    
    public void DrawTabs()
    {
        using (var tabBar = ImRaii.TabBar("MainMenuTabs###", ImGuiTabBarFlags.Reorderable))
        {
            if (!tabBar)
            {
                Services.Log.Error("[ERROR]: ImRaii TabBar isn't valid. Please tell me about this error.");
                return;
            }

            if (ImGui.BeginTabItem("Games"))
            {
                TabGames();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("Settings"))
            {
                TabSettings();
                ImGui.EndTabItem();
            }
            if (ImGui.BeginTabItem("High-Scores"))
            {
                TabHighScores();
                ImGui.EndTabItem();
            }
        }
    }
    
    public void DrawGamesTab()
    {
        ImGui.BeginChild("Game Buttons");
        
        DrawGame(new Vector2(100, 50), EGame.PuzzlePanel, "Puzzle Panel");
        DrawGame(new Vector2(365 + MenuImgPaddingX, 50), EGame.MemoryMatch, "Memory Match");
        
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
    
    //  Render
    public override void Draw()
    {
        DrawTabs();
        
        Services.Log.Information(plugin.GetPlayerName()!);
    }
}
