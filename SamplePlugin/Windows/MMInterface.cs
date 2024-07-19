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

//  This is the ugly-ist shit i've ever seen and almost the ugly-ist shit I've ever written.
    public override void Draw()
    {
        if (!bGameStart)
        {
            ImGui.Text("Board Size");
            if (ImGui.Button("4"))
            {
                board.cardCount = EBoardSize.Four;
                
            }
            
            ImGui.SameLine();
            
            if (ImGui.Button("8"))
            {
                board.cardCount = EBoardSize.Eight;
                board.ResetBoard();
            }
        
            ImGui.NewLine();

            if (ImGui.Button("12"))
            { 
                board.cardCount = EBoardSize.Twelve;
                board.ResetBoard();
            }
            
            ImGui.SameLine();
            
            if (ImGui.Button("16"))
            { 
                board.cardCount = EBoardSize.Sixteen;
                board.ResetBoard();
            }
            
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
