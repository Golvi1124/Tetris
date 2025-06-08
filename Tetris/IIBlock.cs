using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;
public class IIBlock : Block
{
    private readonly Position[][] tiles = new Position[][]
    {
        new Position[] { new(0, 1), new(1, 1) }, // Vertical
        new Position[] { new(1, 0), new(1, 1) }, // Horizontal
        new Position[] { new(0, 0), new(1, 0) }, // Vertical flipped
        new Position[] { new(0, 0), new(0, 1) }  // Horizontal flipped
    };

    public override int Id => 8;
    protected override Position StartOffset => new Position(0, 4); // Adjust to center it
    protected override Position[][] Tiles => tiles;
}
