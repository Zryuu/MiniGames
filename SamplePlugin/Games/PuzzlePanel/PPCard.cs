using System;
using System.IO;
using System.Numerics;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Data;
using Dalamud.Interface.Textures;
using FFXIVClientStructs.FFXIV.Component.GUI;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

namespace SamplePlugin.Games.PuzzlePanel;


public class PPCard
{
    public enum EFlipped { Mushroom, Feather};
    public EFlipped FaceSide;
    public bool bisDisplay;
    private readonly ISharedImmediateTexture MushroomTex;
    private readonly ISharedImmediateTexture FeatherTex;
    private ISharedImmediateTexture currentTex;
    public bool IsFlipped { get; private set; }

    
    public PPCard(bool display)
    {
        bisDisplay = display;
     

        
        FaceSide = EFlipped.Mushroom;
        InitCardsSide(FaceSide);
        
        MushroomTex = Services.TextureProvider.GetFromGameIcon(
            new GameIconLookup(62114, false, true, null));
        FeatherTex = Services.TextureProvider.GetFromGameIcon(
            new GameIconLookup(62117, false, true, null));
        currentTex = MushroomTex;
        IsFlipped = false;
    }

    public void InitCardsSide(EFlipped side)
    {
        if (bisDisplay)
        {
            FaceSide = EFlipped.Mushroom;
            currentTex = MushroomTex;
        }
        
        FaceSide = side;
    }

    public void SwapFaceSide()
    {
        if (bisDisplay)
        {
            return;
        }
        
        IsFlipped = !IsFlipped;
        
        switch (FaceSide)
        {
            case EFlipped.Mushroom :
                FaceSide = EFlipped.Feather;
                currentTex = FeatherTex;
                break;
            case EFlipped.Feather :
                FaceSide = EFlipped.Mushroom;
                currentTex = MushroomTex;
                break;
        }
    }

    public ISharedImmediateTexture GetTex()
    {
        return currentTex;
    }
}
