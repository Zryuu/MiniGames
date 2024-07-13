namespace SamplePlugin.Games.PuzzlePanel;

public class PPGameState
{
    private PPBoard PlayBoard;
    private PPBoard DisplayBoard;
    
    public PPGameState()
    {
        
        
        DisplayBoard.bisDisplayBoard = true;
        PlayBoard.bisDisplayBoard = false;
    }

    public void SetBoardSize(int h, int w)
    {
        PlayBoard.SetBoardSize(h, w);
        DisplayBoard.SetBoardSize(h, w);
    }

    public void CreateBoards()
    {
        DisplayBoard.CreateDisplayBoard();
        PlayBoard.CreateBoard();
    }
    
    
}
