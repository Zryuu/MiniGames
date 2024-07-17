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
    public int boardSize, width, cardChance, CardAmount;
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

    public void RandomizeCardsFace(int i)
    {
        Random random = new Random();
        int rand;
        
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
    }
    
    public void DrawBoard()
    {

        
        
        switch (cardCount)
        {
            case EBoardSize.Four:
                for (int i = 0; i < 4; i++)
                {
                    RandomizeCardsFace(i);
                    
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
