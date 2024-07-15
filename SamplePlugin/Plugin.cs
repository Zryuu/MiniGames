using System;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Data;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using SamplePlugin.Windows;

namespace SamplePlugin;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    
    public HighScoreManager HighScoreManager { get; private set; }
    
    private const string PPComand = "/PP";
    private readonly string? LocPlayerName;

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("SamplePlugin");
    private ConfigWindow ConfigWindow { get; init; }
    private Interface Interface { get; init; }
    private PpInterface PpInterface { get; set; }
    
    
    public Plugin()
    {
        PluginInterface.Create<Services>();
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        HighScoreManager = new HighScoreManager("Scores.json");
        
        ConfigWindow = new ConfigWindow(this);
        Interface = new Interface(this);
        PpInterface = new PpInterface(this);
        
        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(Interface);
        WindowSystem.AddWindow(PpInterface);
        
        

        PluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;

        LocPlayerName = Services.ClientState.LocalPlayer!.Name.ToString();
        
    }

    public string? GetPlayerName()
    {
        var name = LocPlayerName ?? "Player Name Null";
        return name;
    }
    
    public void OnGameEnd(EGame game, int score)
    {
        HighScoreManager.AddHighScore(game, GetPlayerName() ,score);
    }
    
    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();

        ConfigWindow.Dispose();
        Interface.Dispose();
        
        
    }

    private void OnCommand(string command, string args)
    {   
        TogglePPUI();
    }

    private void DrawUI() => WindowSystem.Draw();

    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => Interface.Toggle();
    public void TogglePPUI() => PpInterface.Toggle();
}
