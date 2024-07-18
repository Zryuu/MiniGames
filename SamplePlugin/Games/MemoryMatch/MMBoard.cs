﻿using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Utility;
using FFXIVClientStructs.FFXIV.Common.Lua;
using ImGuiNET;
using Lumina.Excel.GeneratedSheets;

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
    public int boardSize, width, cardChance, CardAmount, Rand, Rand2;
    private readonly int height = 2;
    private uint soundindex;
    private EBoardSize cardCount;
    private int[] cardTypes;

    public MMCard[] cards;


    public MMBoard()
    {
        cardCount = EBoardSize.Twelve;
        SetBoardSize(cardCount);
        
        cards = new MMCard[boardSize];
        width = boardSize / 2;
        
        CreateBoard();
        SetRandomCardType();
    }

    public void ResetBoard()
    {
        SetBoardSize(cardCount);
        CreateBoard();

        for (int i = 0; i < cardTypes.Length; i++)
        {
            //cardTypes[i] = 0;
        }
        
        SetRandomCardType();
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
            EBoardSize.Sixteen => 16,
            _ => throw new ArgumentOutOfRangeException(nameof(newSize), newSize, null)
        };
        
        CardAmount = boardSize / 2;
        cardTypes = new int[16];
    }
    
    //  Creates a 2x2, 2x4, 2x6, 2x8 board.
    public void CreateBoard()
    {
        for (int column = 0; column < width; column++)
        {
            for (int row = 0; row < height; row++)
            {
                int index = (row * width) + column;
                cards[index] = new MMCard
                {
                    bTypeSet = false
                };
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

    //  Randomize Card Types.
    public void SetRandomCardType()
    {
        //  Inits Random.
        Random random = new Random();
        
        //  Loops through every card
        foreach (var card in cards.Select((value, i) => new {i, value}))
        {
            var value = card.i;
            var cardIndex = card.value;

            //  Checks if card was already set....prob useless
            if (cardIndex.bTypeSet)
            {
                return;
            }
            
            //  Random Int
            Rand = random.Next(CardAmount);

            //  Checks if Rand is a full index already. If so ++. Loop back to 0 if Rand = last index
            while (true)
            {
                if (cardTypes[Rand] == 2)
                {
                    if (Rand == CardAmount)
                    {
                        Rand = 0;
                        continue;
                    }
                    
                    Rand++;
                }
                else
                {
                    break;
                }
                
            }
            
            //  Type cast. Changing int to Enum value
            var chosenCard = (ECardType)Rand;
            
            //  Sets CardType
            cardIndex.SetCardType(chosenCard);
            
            //  Sets Type changed bool
            cardIndex.bTypeSet = true;
            
            //  Adds card to cardType array index.
            cardTypes[Rand]++;
        }
    }
    
    public void DrawBoard()
    {
        foreach (var card in cards.Select((value, i) => new {i, value}))
        {
            var value = card.value;
            var index = card.i;
            float xPos = (ImGui.GetWindowWidth() / 2 - (2 * 100)) + ((index % CardAmount) * (100 + 10));
            float yPos = (ImGui.GetWindowHeight() / 2) + ((index / CardAmount) * (100 + 10));
                    
            ImGui.SetCursorPos(ImGuiHelpers.ScaledVector2(xPos, yPos));
            ImGui.PushID(index);
            if (ImGui.ImageButton(value.GetTex().GetWrapOrEmpty().ImGuiHandle, new Vector2(100, 100)))
            {
                value.SwapFace();
                value.bFlipped = true;
            }
        }
    }
}
