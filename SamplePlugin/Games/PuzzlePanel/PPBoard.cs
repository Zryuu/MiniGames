using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Lumina.Excel.GeneratedSheets2;
using SamplePlugin.Games.PuzzlePanel;

namespace SamplePlugin.Games.PuzzlePanel;

public class PPBoard()
{
    
    public int Height, Width;
    public float CardSize;
    public PPCard[] DisplayCards;
    public PPCard[] Cards;
    public PPCard Card;
    public bool bisDisplayBoard;

    public PPBoard(bool display, int height, int width) : this()
    {
        
        bisDisplayBoard = display;
        Height = height;
        Width = width;
        
        Cards = new PPCard[Height * Width];
        DisplayCards = new PPCard[Height * Width];
        
        CreateBoard();
        CreateDisplayBoard();
    }
    
    public int GetBoardHeight()
    {
        return Height;
    }
    
    public int GetBoardWidth()
    {
        return Width;
    }
    
    public void SetBoardSize(int h, int w)
    {
        if (h >= 3 && w >= 3)
        {
            Height = h;
            Width = w;
        }
    }

    public void CreateBoard()
    {
        for (int i = 0; i < Height * Width; i++)
        {
            Card = new PPCard(false);
            Cards[i] = Card;
        }
            
    }

    public void SetPlayerCardsInit()
    {
        foreach (var t in Cards)
        {
            if (t.FaceSide != PPCard.EFlipped.Mushroom)
            {
                t.SwapFaceSide();
            }
        }
    }
    
    public PPCard GetCardFromCards(int i)
    {
        return Cards[i];
    }
    
    public PPCard GetDisplayCardFromDisplayCards(int i)
    {
        return DisplayCards[i];
    }
    
    
    public void CreateDisplayBoard()
    {
        for (int column = 0; column < Width; column++)
        {
            for (int row = 0; row < Height; row++)
            {
                int index = (row * Width) + column;
                DisplayCards[index] = new PPCard(bisDisplayBoard);
            }
        }
    }
    
    public void SetDisplayCards()
    {
        Random random = new Random();
        int rand;

        for (int column = 0; column < Width; column++)
        {
            for (int row = 0; row < Height; row++)
            {
                int index = (row * Width) + column;
                rand = random.Next(100);
                PPCard currentCard = DisplayCards[index];

                if (rand > 75)
                {
                    currentCard.SwapFaceSide();
                    
                    int[] adjacentIndices = GetAdjacentCardIndices(index);
                    Services.Log.Information(index.ToString());
                    foreach (int adjacentIndex in adjacentIndices)
                    {
                        Cards[adjacentIndex].SwapFaceSide();
                    }
                }
            }
        }
    }
    
    public int[] GetAdjacentCardIndices(int index)
    {
        List<int> adjacentIndices = new List<int>();

        int row = index / Width;
        int col = index % Width;

        // Left
        if (col > 0)
            adjacentIndices.Add(index - 1);
        // Right
        if (col < Width - 1)
            adjacentIndices.Add(index + 1);
        // Top
        if (row > 0)
            adjacentIndices.Add(index - Width);
        // Bottom
        if (row < Height - 1)
            adjacentIndices.Add(index + Width);

        return adjacentIndices.ToArray();
    }
    
    public bool CheckBoardsMatch()
    {
        for (int i = 0; i < Height * Width; i++)
        {
            if (Cards[i].FaceSide != DisplayCards[i].FaceSide || Cards[i].IsFlipped != DisplayCards[i].IsFlipped)
            {
                return false;
            }
        }
        return true;
    }
    
}
