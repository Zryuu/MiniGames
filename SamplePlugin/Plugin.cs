using System;
using System.Collections.Generic;
using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using Dalamud.Data;
using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin.Services;
using ImGuiNET;
using SamplePlugin.Windows;

namespace SamplePlugin;

public sealed class Plugin : IDalamudPlugin
{
    [PluginService] internal static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    
    public HighScoreManager HighScoreManager { get; private set; }
    
    private readonly string? LocPlayerName;
    public static float DeltaTime => ImGui.GetIO().DeltaTime * 1000;

    public Configuration Configuration { get; init; }

    public readonly WindowSystem WindowSystem = new("SamplePlugin");
    private ConfigWindow ConfigWindow { get; init; }
    private Interface Interface { get; init; }
    private PpInterface PpInterface { get; set; }
    private MMInterface MmInterface { get; set; }


    public Plugin()
    {
        PluginInterface.Create<Services>();
        Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
        
        HighScoreManager = new HighScoreManager("Scores.json");
        
        ConfigWindow = new ConfigWindow(this);
        Interface = new Interface(this);
        PpInterface = new PpInterface(this);
        MmInterface = new MMInterface(this);
        
        WindowSystem.AddWindow(ConfigWindow);
        WindowSystem.AddWindow(Interface);
        WindowSystem.AddWindow(PpInterface);
        WindowSystem.AddWindow(MmInterface);

        InitializeCommands();

        PluginInterface.UiBuilder.Draw += DrawUI;

        // This adds a button to the plugin installer entry of this plugin which allows
        // to toggle the display status of the configuration ui
        PluginInterface.UiBuilder.OpenConfigUi += ToggleConfigUI;

        // Adds another button that is doing the same but for the main ui of the plugin
        PluginInterface.UiBuilder.OpenMainUi += ToggleMainUI;

        if (Services.ClientState.LocalPlayer != null)
        {
            LocPlayerName = Services.ClientState.LocalPlayer!.Name.ToString();
        }
    }

    //  Gets Local Players name for High-score
    public string? GetPlayerName()
    {
        var name = LocPlayerName ?? "Player Name Null";
        return name;
    }
    
    public const string MenuCommand = "/MGM";
    public const string PuzzlePanel = "/PP";
    public const string MemoryMatch = "/MM";
    
    private readonly Dictionary<string, CommandInfo> _commands = new();

    //  Init Commands
    private void InitializeCommands()
    {
        _commands[MenuCommand] = new CommandInfo(MenuCI)
        {
            HelpMessage = "Use to open the MiniGame menu.",
            ShowInHelp = true,
                
        };
        _commands[PuzzlePanel] = new CommandInfo(PPCI)
        {
            HelpMessage = "Use to open the Puzzle Panel game.",
            ShowInHelp = true,
                
        };
        _commands[MemoryMatch] = new CommandInfo(MMCI)
        {
            HelpMessage = "Use to open the Memory Match game.",
            ShowInHelp = true,
                
        };

        foreach (var (command, info) in _commands)
            Services.CommandManager.AddHandler(command, info);
    }
    private void DisposeCommands()
    {
        foreach (var command in _commands.Keys)
            Services.CommandManager.RemoveHandler(command);
    }
    
    public void OnGameEnd(EGame game, int? score, int? time)
    {
        HighScoreManager.AddHighScore(game, GetPlayerName() ,score, time);
    }
    
    public void Dispose()
    {
        WindowSystem.RemoveAllWindows();
        ConfigWindow.Dispose();
        Interface.Dispose();
        DisposeCommands();
    }

/*    
********************************************************    
**                                                    **  
 *              COMMANDS                              *  
**                                                    **  
********************************************************  
*/  

    private void DrawUI() => WindowSystem.Draw();
    
    private void MenuCI(string command, string args) { ToggleMainUI(); }
    private void PPCI(string command, string args) { TogglePPUI(); }
    private void MMCI(string command, string args) { ToggleMMUI(); }
    public void ToggleConfigUI() => ConfigWindow.Toggle();
    public void ToggleMainUI() => Interface.Toggle();
    public void TogglePPUI() => PpInterface.Toggle();
    public void ToggleMMUI() => MmInterface.Toggle();
}
