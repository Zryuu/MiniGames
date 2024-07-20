using System;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Interface.Textures;
using Dalamud.Interface.Textures.Internal;

namespace SamplePlugin.Games.MemoryMatch;

public enum ECardType
{
    Blue,
    Red,
    Green,
    Yellow,
    Purple,
    Orange,
    White,
    Black
}

enum ECardFace
{
    FaceUp,
    FaceDown
}

public class MMCard
{
    public ECardType cardType;
    private ECardFace cardFace;
    public bool bTypeSet = false;
    public bool bFlipped = false;
    
    //  Set icons
    private readonly ISharedImmediateTexture blueFace;
    private readonly ISharedImmediateTexture redFace;
    private readonly ISharedImmediateTexture greenFace;
    private readonly ISharedImmediateTexture yellowFace;
    private readonly ISharedImmediateTexture purpleFace;
    private readonly ISharedImmediateTexture orangeFace;
    private readonly ISharedImmediateTexture whiteFace;
    private readonly ISharedImmediateTexture blackFace;
    private readonly ISharedImmediateTexture backSide;
    private readonly ISharedImmediateTexture matchedOverlay;
    private readonly ISharedImmediateTexture currentFlippedOverlay;
    
    private  ISharedImmediateTexture currentTex;

    public MMCard()
    {
        
        
        //  Find good textures for this
        blueFace       = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060501, false, true, null));
        redFace        = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060501, false, true, null));
        greenFace      = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060501, false, true, null));
        yellowFace     = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060501, false, true, null));
        purpleFace     = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060501, false, true, null));
        orangeFace     = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060501, false, true, null));
        whiteFace      = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060501, false, true, null));
        blackFace      = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060501, false, true, null));
       
        //  Backside texture
        backSide       = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060958, false, true, null));
        
        //  Texture overlay to show cards were matched.....maybe grab the checkmark overlay used for items/character menu.
        matchedOverlay = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060957, false, true, null));
        currentFlippedOverlay = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(060959, false, true, null));
            
        currentTex = backSide;
        cardFace = ECardFace.FaceDown;
    }
    
    private ISharedImmediateTexture GetFaceTexture()
    {
        return cardType switch
        {
            ECardType.Blue    => blueFace,
            ECardType.Red     => redFace,
            ECardType.Green   => greenFace,
            ECardType.Yellow  => yellowFace,
            ECardType.Purple  => purpleFace,
            ECardType.Orange  => orangeFace,
            ECardType.White  => whiteFace,
            ECardType.Black  => blackFace,
            _ => backSide
        };
    }

    public void SetCardType(ECardType newValue)
    {
        cardType = newValue;
    }
    
    public void SwapFace()
    {
        cardFace = cardFace switch
        {
            ECardFace.FaceDown => ECardFace.FaceUp,
            ECardFace.FaceUp => ECardFace.FaceDown,
        };

        currentTex = ((cardFace == ECardFace.FaceDown) ? backSide : GetFaceTexture());
    }
    
    public ISharedImmediateTexture GetTex()
    {
        return currentTex;
    }

}
