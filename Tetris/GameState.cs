using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;

public class GameState
{
    private Block currentBlock; // the current block being played

    public Block CurrentBlock
    {
        get => currentBlock; // property to get the current block
        private set
        {
            currentBlock = value; // set the current block
            currentBlock.Reset(); // reset the block to its initial position and rotation state
        }
    }

    public GameGrid GameGrid { get; } // the game grid where the blocks are placed
    public BlockQueue BlockQueue { get; } // the queue of blocks to be played
    public bool GameOver { get; private set; } // flag to indicate if the game is over

    public GameState()
    {
        GameGrid = new GameGrid(22, 10); // initialize GameGrid with borders
        BlockQueue = new BlockQueue(); // initialize block queue
        CurrentBlock = BlockQueue.GetAndUpdate();
    }

    private bool BlockFits() //checks if current block is in legal position
    {
        foreach (Position p in CurrentBlock.TilePositions())
        {
            if (!GameGrid.IsEmpty(p.Row, p.Column))
            {
                return false;
            }
        }
        return true;
    }

    // Methods to rotate blocks and only if it is possible
    public void RotateBlockCW()
    {
        CurrentBlock.RotateCW();

        if (!BlockFits())
        {
            CurrentBlock.RotateCCW(); // revert rotation if it doesn't fit
        }
    }

    public void RotateBlockCCW()
    {
        CurrentBlock.RotateCCW();
        if (!BlockFits())
        {
            CurrentBlock.RotateCW();
        }
    }

    // Methods to move blocks left/right and only if it is possible
    public void MoveBlockLeft()
    {
        CurrentBlock.Move(0, -1); // move left

        if (!BlockFits())
        {
            CurrentBlock.Move(0, 1); // revert move if it doesn't fit
        }
    }

    public void MoveBlockRight()
    {
        CurrentBlock.Move(0, 1); // move right

        if (!BlockFits())
        {
            CurrentBlock.Move(0, -1); // revert move if it doesn't fit
        }
    }

    private bool IsGameOver() // if the first two rows are not empty, the game is over
    {
        return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
    }

    private void PlaceBlock() // places the current block on the grid
    {
        foreach (Position p in CurrentBlock.TilePositions())
        {
            GameGrid[p.Row, p.Column] = CurrentBlock.Id; // set the grid cell to the block's ID
        }

        GameGrid.ClearFullRows(); // clear any full rows in the grid

        if (IsGameOver())
        {
            GameOver = true; // set game over flag if the game is over
        }
        else
        {
            CurrentBlock = BlockQueue.GetAndUpdate(); // get the next block from the queue
        }
    }
    public void MoveBlockDown() // moves the current block down
    {
        CurrentBlock.Move(1, 0); // move down

        if (!BlockFits()) // if it doesn't fit, place the block on the grid
        {
            CurrentBlock.Move(-1, 0); // revert move if it doesn't fit
            PlaceBlock(); // place the block on the grid
        }
    }
}
