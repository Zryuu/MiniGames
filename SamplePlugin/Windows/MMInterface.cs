using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using SamplePlugin.Games.MemoryMatch;

namespace SamplePlugin.Windows;

public class MMInterface : Window, IDisposable
{

    
    private MMBoard board;
    private Timer StopWatch;
    private bool bGameStart = false;
    
    public MMInterface(Plugin plugin): base("Memory Match", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {

        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(506, 736),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        board = new MMBoard();
        StopWatch = new Timer();
        InitGame(); 
    }

    private void InitGame()
    {
        board.SetBoardSize(EBoardSize.Four);

    }
    
    public void Dispose() { }


    public override void Draw()
    {
        if (!bGameStart)
        {
            ImGui.Text("Board Size");
            ImGui.Button("4");
            ImGui.SameLine();
            ImGui.Button("8");
        
            ImGui.NewLine();
        
            ImGui.Button("12");
            ImGui.SameLine();
            ImGui.Button("16");
            
            ImGui.SetCursorPos(ImGuiHelpers.ScaledVector2(ImGui.GetWindowWidth() / 2, ImGui.GetWindowHeight() / 2));
            if (ImGui.Button("START"))
            {
                bGameStart = true;
                StopWatch.Start();
            }
        }
        else
        {
            if (ImGui.Button("Reset"))
            {
                board.ResetBoard();
                StopWatch.Reset();
                bGameStart = false;
            }
            
            ImGui.SetCursorPosX(ImGui.GetWindowWidth() / 2);
            ImGui.Text(StopWatch.GetElapsedTime());
        
            board.DrawBoard();
        }
    }
}
