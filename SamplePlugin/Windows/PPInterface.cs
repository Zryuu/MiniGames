using System;
using System.Numerics;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Client.UI;
using ImGuiNET;
using Lumina.Data.Files;
using SamplePlugin.Games.PuzzlePanel;


namespace SamplePlugin.Windows;

public class PpInterface : Window, IDisposable
{
    private Plugin plugin;
    private PPBoard board;
    private PPCard[] cards;
    private PPCard[] displayCards;
    
    private bool[] buttonClicked;
    private uint currentSound, soundindex;
    private float boardSpacing, padding, buttonSize, totalWidth, totalHeight, windowWidth, windowHeight;
    private int columns, rows, boardsize, score;
    
    
    public PpInterface(Plugin plugin)
        : base("Puzzle Panel", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.plugin = plugin;
        
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(506, 736),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };
        
        boardsize = 6;
        board = new PPBoard(false, boardsize, boardsize);
        
        boardSpacing = 20.0f;
        padding = 10f;
        buttonSize = 40f;
        soundindex = 0;
        
        InitGame();
    }


    public void InitGame()
    {
        
        
        board.SetBoardSize(boardsize, boardsize);
        cards = new PPCard[board.Cards.Length];
        displayCards = new PPCard[board.DisplayCards.Length];
        buttonClicked = new bool[board.Cards.Length];

        columns = board.Width;
        rows = board.Height;
        
        for (int i = 0; i < board.Cards.Length; i++)
        {
            cards[i] = board.GetCardFromCards(i);
            buttonClicked[i] = false;
        }
        
        for (int i = 0; i < board.DisplayCards.Length; i++)
        {
            displayCards[i] = board.GetDisplayCardFromDisplayCards(i);
        }
        
        board.SetDisplayCards();
        board.SetPlayerCardsInit();
        
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

            if (ImGui.ImageButton(cards[i].GetTex().GetWrapOrEmpty().ImGuiHandle, new Vector2(buttonSize, buttonSize)))
            {
                cards[i].SwapFaceSide();
                UIModule.PlaySound(currentSound);
                
                int[] adjacentIndices = board.GetAdjacentCardIndices(i);
                foreach (int adjacentIndex in adjacentIndices)
                {
                    cards[adjacentIndex].SwapFaceSide();
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
            ImGui.ImageButton(displayCards[i].GetTex().GetWrapOrEmpty().ImGuiHandle,
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
        

        if (!board.CheckBoardsMatch())
        {
            if (ImGui.Button($"Sound Effect: {currentSound}"))
            {
                
                soundindex++;
                currentSound = soundindex;
                
                //  Some reason this shit doesnt work if merged
                if (soundindex == 18)
                {
                    soundindex = 22;
                }
                if (soundindex == 72)
                {
                    soundindex = 0;
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
                score = boardsize * 1000;
                plugin.OnGameEnd(EGame.PuzzlePanel, score);
                InitGame();
            }
        }
    }
}
