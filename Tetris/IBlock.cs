using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;

public class IBlock : Block // IBlock class inherits from Block class
{
    private Position[][] tiles => new Position[][] // first store tile positions for 4 positions of the IBlock in a 2D array
    {
        new Position[] { new(1,0), new(1,1), new(1,2), new(1,3) }, // State 0: Horizontal line
        new Position[] { new(0,2), new(1,2), new(2,2), new(3,2) }, // State 1: Vertical line
        new Position[] { new(2,0), new(2,1), new(2,2), new(2,3) }, // State 2: Horizontal line (shifted)
        new Position[] { new(0,1), new(1,1), new(2,1), new(3,1) }  // State 3: Vertical line (shifted)
    };
    //next fill out properties for the IBlock class
    public override int Id => 1; // unique identifier for the IBlock
    protected override Position StartOffset => new Position(-1, 3); // starting position of the block so it spawns in the middle of the top row
    protected override Position[][] Tiles => tiles; //return tiles array from above
}