﻿using Tetris.Models;

namespace Tetris.Blocks;

internal class LBlock : Block
{
    private readonly Position[][] tiles = new Position[][]
    {
        new Position[] { new(0,2), new(1,0), new(1,1), new(1,2) },
        new Position[] { new(0,1), new(1,1), new(2,1), new(2,2) },
        new Position[] { new(1,0), new(1,1), new(1,2), new(2,0) },
        new Position[] { new(0,0), new(0,1), new(1,1), new(2,1) }
    };
    public override int Id => 3; // unique identifier for the LBlock
    protected override Position StartOffset => new Position(0, 3); // starting position of the block
    protected override Position[][] Tiles => tiles; // return the tiles array defined above
}