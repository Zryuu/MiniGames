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
    
    public MMInterface(Plugin plugin): base("Memory Match", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(506, 736),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        board = new MMBoard();
        InitGame(); 
    }

    private void InitGame()
    {
        board.SetBoardSize(EBoardSize.Four);

    }

    public void DrawBoard()
    {
        for (int i = 0; i < board.cards.Length; i++)
        {
            ImGui.ImageButton(board.cards[i].GetTex().GetWrapOrEmpty().ImGuiHandle,
                              new Vector2(100,100));
        }
    }
    
    public void Dispose() { }


    public override void Draw()
    {
        board.DrawBoard();
        //DrawBoard();
        
    }
}
