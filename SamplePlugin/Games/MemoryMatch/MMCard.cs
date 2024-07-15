using Dalamud.Interface.Textures;

namespace SamplePlugin.Games.MemoryMatch;


enum ECardType
{
    Blue,
    Red,
    Green,
    Yellow
}

enum ECardFace
{
    FaceUp,
    FaceDown
}

public class MMCard
{
    private ECardType cardType;
    private ECardFace cardFace;
    private readonly ISharedImmediateTexture BlueFace;
    private readonly ISharedImmediateTexture RedFace;
    private readonly ISharedImmediateTexture GreenFace;
    private readonly ISharedImmediateTexture YellowFace;

    public MMCard()
    {
        
        
        
    }


    private void SwapFace()
    {
        cardFace = cardFace switch
        {
            ECardFace.FaceDown => ECardFace.FaceUp,
            ECardFace.FaceUp => ECardFace.FaceDown,
        };
    }

}
