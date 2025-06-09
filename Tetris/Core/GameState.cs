using Tetris.Models;

namespace Tetris.Core;

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

            for (int i = 0; i < 2; i++)
            {
                currentBlock.Move(1, 0);

                if (!BlockFits())
                {
                    currentBlock.Move(-1, 0);
                }
            }
        }
    }

    public GameGrid GameGrid { get; } // the game grid where the blocks are placed
    public BlockQueue BlockQueue { get; } // the queue of blocks to be played
    public bool GameOver { get; private set; } // flag to indicate if the game is over

    public int Score { get; private set; } // property to hold the score of the game

    public Block HeldBlock { get; private set; } // property to hold the held block, if any
    public bool CanHold { get; private set; } // flag to indicate if the player can hold a block

    public GameState()
    {
        GameGrid = new GameGrid(22, 10); // initialize GameGrid with borders
        BlockQueue = new BlockQueue(); // initialize block queue
        CurrentBlock = BlockQueue.GetAndUpdate();
        CanHold = true;
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

    public void HoldBlock()
    {
        if (!CanHold) return; // if holding is not allowed, do nothing

        if (HeldBlock == null) // if no block is held, hold the current block
        {
            HeldBlock = CurrentBlock;
            CurrentBlock = BlockQueue.GetAndUpdate(); // get the next block from the queue
        }
        else // if a block is already held, swap it with the current block
        {
            Block tmp = CurrentBlock;
            CurrentBlock = HeldBlock; // set the current block to the held block
            HeldBlock = tmp; // set the held block to the previous current block
        }

        CanHold = false; // set holding to false, so the player cannot hold another block until the next move
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

        Score += GameGrid.ClearFullRows(); // clear any full rows in the grid + update the score

        if (IsGameOver())
        {
            GameOver = true; // set game over flag if the game is over
        }
        else
        {
            CurrentBlock = BlockQueue.GetAndUpdate(); // get the next block from the queue
            CanHold = true; // allow holding a block again
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

    private int TileDropDistance(Position p)
    {
        int drop = 0;

        while (GameGrid.IsEmpty(p.Row + drop + 1, p.Column))
        {
            drop++;
        }

        return drop; // returns the distance the tile can drop
    }

    public int BlockDropDistance()
    {
        int drop = GameGrid.Rows; // start with the maximum possible drop distance

        foreach (Position p in CurrentBlock.TilePositions())
        {
            drop = Math.Min(drop, TileDropDistance(p)); // find the minimum drop distance for all tiles
        }

        return drop; // returns the minimum drop distance for the entire block
    }

    public void DropBlock() // drops the current block to the bottom
    {
        CurrentBlock.Move(BlockDropDistance(), 0); // move the block down by the calculated drop distance
        PlaceBlock(); // place the block on the grid after dropping it
    }
}