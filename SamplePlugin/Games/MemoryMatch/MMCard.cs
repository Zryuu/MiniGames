using System;
using Dalamud.Game.ClientState.JobGauge.Enums;
using Dalamud.Interface.Textures;

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
    private readonly ISharedImmediateTexture blueFace   = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(62115, false, true, null));
    private readonly ISharedImmediateTexture redFace    = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(62114, false, true, null));
    private readonly ISharedImmediateTexture greenFace  = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(62113, false, true, null));
    private readonly ISharedImmediateTexture yellowFace = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(62112, false, true, null));
    private readonly ISharedImmediateTexture purpleFace = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(62111, false, true, null));
    private readonly ISharedImmediateTexture orangeFace = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(62110, false, true, null));
    private readonly ISharedImmediateTexture whiteFace  = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(62109, false, true, null));
    private readonly ISharedImmediateTexture blackFace  = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(62108, false, true, null));
    private readonly ISharedImmediateTexture backSide   = Services.TextureProvider.GetFromGameIcon(new GameIconLookup(62107, false, true, null));
    
    private  ISharedImmediateTexture currentTex;

    public MMCard()
    {
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
