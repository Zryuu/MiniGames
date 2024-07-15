using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.UI;
using ImGuiNET;
using Lumina.Data.Files;
using SamplePlugin.Games.PuzzlePanel;


namespace SamplePlugin.Windows;

public class PPInterface : Window, IDisposable
{
    
    private PPBoard Board;
    private PPCard[] Cards;
    private PPCard[] DisplayCards;
    private bool[] buttonClicked;
    private Vector2 defaultSize = new Vector2(600, 400);
    private uint currentSound, Soundindex;
    private float boardSpacing, padding, buttonSize, totalWidth, totalHeight, windowWidth, windowHeight;
    private int columns, rows, boardsize;
    
    
    public PPInterface(Plugin plugin)
        : base("Puzzle Panel", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(506, 736),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        
        boardsize = 6;
        Board = new PPBoard(false, boardsize, boardsize);
        
        boardSpacing = 20.0f;
        padding = 10f;
        buttonSize = 40f;
        Soundindex = 0;
        
        InitGame();
    }


    public void InitGame()
    {
        
        
        Board.SetBoardSize(boardsize, boardsize);
        Cards = new PPCard[Board.Cards.Length];
        DisplayCards = new PPCard[Board.DisplayCards.Length];
        buttonClicked = new bool[Board.Cards.Length];

        columns = Board.Width;
        rows = Board.Height;
        
        for (int i = 0; i < Board.Cards.Length; i++)
        {
            Cards[i] = Board.GetCardFromCards(i);
            buttonClicked[i] = false;
        }
        
        for (int i = 0; i < Board.DisplayCards.Length; i++)
        {
            DisplayCards[i] = Board.GetDisplayCardFromDisplayCards(i);
        }
        
        Board.SetDisplayCards();
        Board.SetPlayerCardsInit();
        
                totalWidth = columns * (buttonSize + padding);
        totalHeight = rows * (buttonSize + padding);

    }

    public void Dispose() { }

    public override void PreDraw()
    {
        base.PreDraw();
    }

    public void DrawPlayBoard(float offX, float offY)
    {
        for (int i = 0; i < boardsize * boardsize; i++)
        {
            if (i % columns == 0 && i > 0) ImGui.NewLine();

            float xPos = offX + ((i % columns) * (buttonSize + padding));
            float yPos = offY + ((i / columns) * (buttonSize + padding));
        
            ImGui.SetCursorPos(ImGuiHelpers.ScaledVector2(xPos, yPos));
            ImGui.PushID(i);

            if (ImGui.ImageButton(Cards[i].GetTex().GetWrapOrEmpty().ImGuiHandle, new Vector2(buttonSize, buttonSize)))
            {
                Cards[i].SwapFaceSide();
                UIModule.PlaySound(currentSound);
                
                int[] adjacentIndices = Board.GetAdjacentCardIndices(i);
                foreach (int adjacentIndex in adjacentIndices)
                {
                    Cards[adjacentIndex].SwapFaceSide();
                }
            }
            
            ImGui.PopID();
        }
    }
    
    public void DrawDisplayBoard(float offX, float offY)
    {
        for (int i = 0; i < boardsize * boardsize; i++)
        {
            if (i % columns == 0 && i > 0) ImGui.NewLine();
            
            float xPos = offX + ((i % columns) * (buttonSize + padding));
            float yPos = offY + ((i / columns) * (buttonSize + padding));
        
            ImGui.SetCursorPos(ImGuiHelpers.ScaledVector2(xPos, yPos));
            ImGui.ImageButton(DisplayCards[i].GetTex().GetWrapOrEmpty().ImGuiHandle,
                              new Vector2(buttonSize, buttonSize));
        }
    }
    
    
    public override void Draw()
    {
        windowWidth = ImGui.GetWindowWidth();
        windowHeight = ImGui.GetWindowHeight();
        
        
        float offsetX1 = (windowWidth - totalWidth) / 2;
        float offsetY1 = (windowHeight - (2 * totalHeight) - boardSpacing) / 2;
        float offsetY2 = offsetY1 + totalHeight + boardSpacing;
        

        if (!Board.CheckBoardsMatch())
        {
            if (ImGui.Button($"Sound Effect: {currentSound}"))
            {
                
                Soundindex++;
                currentSound = Soundindex;
                
                //  Some reason this shit doesnt work if merged
                if (Soundindex == 18)
                {
                    Soundindex = 22;
                }
                if (Soundindex == 72)
                {
                    Soundindex = 0;
                }
                UIModule.PlaySound(currentSound);
                
            }
            
            if (ImGui.Button("Restart?##"))
            {
                InitGame();
                UIModule.PlaySound(50);
            }
            
            if (ImGui.Button("Init Game##"))
            {
                boardsize = 6;
                InitGame();
                UIModule.PlaySound(52);
            }
            
            
            
            ImGui.SetCursorPos(new Vector2((ImGui.GetWindowWidth() / 2) - 25, 20));
            ImGui.Text("Match the boards.");
            
            DrawDisplayBoard(offsetX1, offsetY1);
        
            ImGui.Spacing();
            
            DrawPlayBoard(offsetX1, offsetY2);

            ImGui.SliderInt("##Board Size", ref boardsize, 3, 6,
                            "%d", ImGuiSliderFlags.AlwaysClamp);

        }
        else
        {
            
            ImGui.SetCursorPos(new Vector2(ImGui.GetWindowWidth() / 2, ImGui.GetWindowHeight() / 2));
            ImGui.Text("IT MATCHED!!!");
            ImGui.Spacing();
            UIModule.PlaySound(36);

            if (ImGui.Button("Restart?"))
            {
                InitGame();
            }
        }
    }
}
