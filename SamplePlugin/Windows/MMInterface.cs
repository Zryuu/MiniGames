using System;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using SamplePlugin.Games.MemoryMatch;

namespace SamplePlugin.Windows;

public class MMInterface : Window, IDisposable
{
    
    public MMInterface(Plugin plugin): base("Puzzle Panel", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
     
        
    }
    
    public void Dispose() { }


    public override void Draw()
    {
        
        
        
    }
}
