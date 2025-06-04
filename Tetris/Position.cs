using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;

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