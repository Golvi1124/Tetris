﻿using Tetris.Models;

namespace Tetris.Blocks;

public class OBlock : Block
{
    private readonly Position[][] tiles = new Position[][]
    {
        new Position[] { new(0,0), new(0,1), new(1,0), new(1,1) }, // just one since it is a square block
    };

    public override int Id => 4;
    protected override Position StartOffset => new Position(0, 4);
    protected override Position[][] Tiles => tiles; // return the tiles array defined above
}