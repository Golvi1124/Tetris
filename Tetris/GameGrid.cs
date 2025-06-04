using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris;

public class GameGrid // this class will hold 2 dimensiaonal array for the game grid
{
    private readonly int[,] grid;
    public int Rows { get; }
    public int Columns { get; }

    public int this[int r, int c] // indexer to access the grid elements
    {
        get => grid[r, c];
        set => grid[r, c] = value;
    }

    public GameGrid(int rows, int columns) // constructor to initialize the grid with given rows and columns
    {
        Rows = rows;
        Columns = columns;
        grid = new int[rows, columns]; // initialize the grid with zeros
    }

    public bool IsInside(int r, int c) // check if the given row and column are inside the grid
    {
        return r >= 0 && r < Rows && c >= 0 && c < Columns;
    }

    public bool IsEmpty(int r, int c) // check if the given cell is empty
    {
        return IsInside(r, c) && grid[r, c] == 0;
    }

    public bool IsRowFull(int r) // check if the given row is full
    {
        for (int c = 0; c < Columns; c++)
        {
            if (grid[r, c] == 0)
                return false;
        }
        return true;
    }

    public bool IsRowEmpty(int r) // check if the given row is empty
    {
        for (int c = 0; c < Columns; c++)
        {
            if (grid[r, c] != 0)
                return false;
        }
        return true;
    }

    private void ClearRow(int r) // clear the given row by setting all its elements to zero
    {
        for (int c = 0; c < Columns; c++)
        {
            grid[r, c] = 0;
        }
    }

    private void MoveRowDown(int r, int numRows) // move the given row down by the specified number of rows
    {
        for (int c = 0; c < Columns; c++)
        {
            grid[r + numRows, c] = grid[r, c];
            grid[r, c] = 0;
        }
    }

    public int ClearFullRows() // clear all full rows and return the number of rows cleared
    {
        int cleared = 0;
        for (int r = Rows - 1; r >= 0; r--) // start from the bottom row
        {
            if (IsRowFull(r))
            {
                ClearRow(r);
                cleared++;
            }
            else if (cleared > 0) // if we have cleared some rows, we need to move the rows above down
            {
                MoveRowDown(r, cleared);
            }
        }
        return cleared;
    }
}
