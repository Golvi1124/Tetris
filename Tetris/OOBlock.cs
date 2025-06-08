using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;
public class OOBlock : Block
{
    private readonly Position[][] tiles = new Position[][]
    {
        new Position[] { new(0, 0) } // Single tile block
    };

    public override int Id => 9;
    protected override Position StartOffset => new Position(0, 4); // Drop in center column
    protected override Position[][] Tiles => tiles;
}

