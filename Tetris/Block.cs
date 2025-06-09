using Tetris.Models;

namespace Tetris;

public abstract class Block // abstract class defines a base "blueprint" for other classes.
{
    protected abstract Position[][] Tiles { get; } // 2d position array representing the tiles of the block in different rotation states
    protected abstract Position StartOffset { get; } // define where block spawns in the grid
    public abstract int Id { get; } // unique identifier for the block

    private int rotationState; // current rotation state of the block
    private Position offset; // current position of the block in the grid

    public Block() // constructor to initialize the block
    {
        offset = new Position(StartOffset.Row, StartOffset.Column); // set the offset to the starting position
    }

    public IEnumerable<Position> TilePositions() // get the positions of the tiles in the current rotation state
    {
        foreach (Position p in Tiles[rotationState])
        {
            yield return new Position(p.Row + offset.Row, p.Column + offset.Column); //yield return azily produces each tile’s absolute position, avoids full list upfront
        }
    }

    public void RotateCW() // rotate the block clockwise 90 degrees
    {
        rotationState = (rotationState + 1) % Tiles.Length; // increment the rotation state
    }

    public void RotateCCW()
    {
        if (rotationState == 0)
        {
            rotationState = Tiles.Length - 1; // wrap around to the last state
        }
        else
        {
            rotationState--; // decrement the rotation state
        }
    }

    public void Move(int rows, int columns) // move the block by the given number of rows and columns
    {
        offset.Row += rows; // update the row offset
        offset.Column += columns; // update the column offset
    }

    public void Reset() // reset the block to its initial position and rotation state
    {
        rotationState = 0; // reset the rotation state
        offset.Row = StartOffset.Row; // reset the row offset
        offset.Column = StartOffset.Column; // reset the column offset
    }
}