namespace Tetris.Models;

public class Position
{
    public int Row { get; set; } // Row index of the position
    public int Column { get; set; } // Column index of the position
    public Position(int row, int column) // Constructor to initialize the position
    {
        Row = row;
        Column = column;
    }
}