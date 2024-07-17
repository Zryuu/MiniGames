using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using ImGuiNET;

namespace SamplePlugin.Games.MemoryMatch;

public enum EBoardSize
{
    Four,
    Eight,
    Twelve,
    Sixteen
}

public class MMBoard
{
    public int boardSize, width, cardChance, CardAmount, Rand;
    private readonly int height = 2;
    private uint soundindex;
    private EBoardSize cardCount;
    private int[] cardTypes;

    public MMCard[] cards;


    public MMBoard()
    {
        cardCount = EBoardSize.Four;
        SetBoardSize(cardCount);
        
        cards = new MMCard[boardSize];
        width = boardSize / 2;
        CardAmount = boardSize / 2;                 //  Divides board by 4 to give the amount of each card.
        cardChance = 100 / ((boardSize / 4) * 4);
        cardTypes = new int[CardAmount];
        
        CreateBoard();
        RandomizeCardsFace();
    }
    
    public int GetBoardSize()
    {
        return boardSize;
    }
    
    public void SetBoardSize(EBoardSize newSize)
    {
        boardSize = newSize switch
        {
            EBoardSize.Four => 4,
            EBoardSize.Eight => 8,
            EBoardSize.Twelve => 12,
            EBoardSize.Sixteen => 16
        };
        
        cardChance = 100 / ((boardSize / 4) * 4);
        CardAmount = boardSize / 2;
        cardTypes = new int[CardAmount];
    }
    
    //  Creates a 2x2, 2x4, 2x6, 2x8 board.
    public void CreateBoard()
    {
        for (int column = 0; column < width; column++)
        {
            for (int row = 0; row < height; row++)
            {
                int index = (row * width) + column;
                cards[index] = new MMCard();
            }
        }
    }

    public MMCard GetCardFromCards(int card)
    {
        return cards[card];
    }
    
    public bool CheckBoardStatus()
    {
        bool isDone = false;

        return isDone;
    }

    //  Not ready yet. Figure out last part.
    public void GetRandomCardType(MMCard currentCard)
    {
        //  Int Array, holds the count of each type of card.

            
        //  Loops through every card
        foreach (var card in cards)
        {
            Random random = new Random();
            int index = 0;
            
            //  random value between 0-100
            Rand = random.Next(100);

            //  loops threw every cardType index and checks if its 2, if so adds to Index counter.
            for (int i = 0; i < cardTypes.Length; i++)
            {
                if (i == 2)
                {
                    index++;
                }
            }

            //  SafeDivide because c# doesnt have one.....
            if (index != 0)
            {
                //  gives an equal chance to each card.
                int cardCahnce = 100 / index;
            }
            else
            {
                //  Dont know what to make this....should prob be a Log print since it should be impossible.
                int cardChance = 50;
            }

            //  idk wtf to do here to set the Enum of the card......Like I get it if its 2, but if its 8 types should I just make 4 if checks?
            if (Rand > cardChance)
            {
                //card.SetCardType(SOMEHOW GET THE CARDS TYPE?????)
            }
            
        }
    }
    
    public void RandomizeCardsFace()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Random random = new Random();
            int rand;

            switch (cardCount)
            {
                case EBoardSize.Four:
                    if (cardTypes[0] == 2)
                    {
                        cards[i].SetCardType(ECardType.Red);
                        cardTypes[1]++;
                    }
                    
                    rand = random.Next(100);
                    if (rand > cardChance)
                    {
                        cards[i].SetCardType(ECardType.Blue);
                        cardTypes[0]++;
                    }
                    else
                    {
                        cards[i].SetCardType(ECardType.Red);
                        cardTypes[1]++;
                    }
                    break;
                
                case EBoardSize.Eight:
                    if (cardTypes[1] == 2)
                    {
                        cards[i].SetCardType(ECardType.Red);
                        cardTypes[1]++;
                    }
                    
                    rand = random.Next(100);
                    if (rand > cardChance)
                    {
                        cards[i].SetCardType(ECardType.Blue);
                        cardTypes[0]++;
                    }
                    else
                    {
                        cards[i].SetCardType(ECardType.Red);
                        cardTypes[1]++;
                    }
                    break;
            }
        }
    }
    
    public void DrawBoard()
    {
        switch (cardCount)
        {
            case EBoardSize.Four:
                for (int i = 0; i < 4; i++)
                {
                    float xPos = (ImGui.GetWindowWidth() / 2 - (2 * 100)) + ((i % CardAmount) * (100 + 10));
                    float yPos = (ImGui.GetWindowHeight() / 2) + ((i / CardAmount) * (100 + 10));
                    
                    ImGui.SetCursorPos(ImGuiHelpers.ScaledVector2(xPos, yPos));
                    ImGui.PushID(i);
                    if (ImGui.ImageButton(cards[i].GetTex().GetWrapOrEmpty().ImGuiHandle, new Vector2(100, 100)))
                    {
                        cards[i].SwapFace();
                    }
                }
                break;
            
        }
    }
}
