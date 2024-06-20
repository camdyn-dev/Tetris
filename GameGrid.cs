using Microsoft.Windows.Themes;
using System.Windows;

namespace Tetris
{
    public class GameGrid
    {
        private readonly int[,] grid; // This is the grid on which we play tetris
        public int Rows { get; }
        public int Columns { get; }
        public int this[int row, int column] // Allows us to easily index and set the grid via "grid[x,y]"
        {
            get => grid[row, column];
            set => grid[row, column] = value;
        }

        public GameGrid(int rows, int columns) // Just in case we want to play a non-traditional form of Tetris with a larger grid
        {
            Rows = rows; // default for Tetris 20
            Columns = columns; // default for Tetris is 10
            grid = new int[rows, columns];
        }

        // Helper methods
        public bool IsCellInsideGrid(int row, int column) // Checks if a given position is within the grid
        {
            return row >= 0 && row < Rows
                && column >= 0 && column < Columns;
            // i.e., greater than zero and less than the grid boundaries we set
        }

        public bool IsCellEmpty(int row, int column) // Checks if a given cell is empty
        {
            return IsCellInsideGrid(row, column)
                && grid[row, column] == 0;
            // i.e., inside the grid and not assigned any value
        }

        public bool IsRowFull(int row) // Checks if a row is full (so we can clear it)
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[row, column] == 0)
                {
                    return false; // If we hit this, there is an unassigned value within the row
                }
            }
            return true; // Otherwise, the row is full
        }

        public bool IsRowEmpty(int row) // Checks if a row is empty
        {
            for (int column = 0; column < Columns; column++)
            {
                if (grid[row, column] != 0)
                {
                    return false; // If we hit this, there is an assigned within the row
                }
            }
            return true; // Otherwise, the row is empty
        }

        // Game methods
        private void ClearRow(int row) // Lets us clear a row (if we fill up an entire row)
        {
            for (int column = 0; column < Columns; column++)
            {
                grid[row, column] = 0;
            }
        }

        private void MoveRowDown(int row, int numRowsDown) // Lets us move a row down (after clearing one)
        {
            for(int column = 0; column < Columns; column++)
            {
                grid[row + numRowsDown, column] = grid[row, column]; // Set a cell x rows down to the current cell
                grid[row, column] = 0; // Set current cell to nothing
            }
        }

        public int ClearFullRows() // This will put together the two previous functions, letting us finally clear a row
        {
            int clearedRows = 0; // Counter for number of rows we clear
            // This will be very important for the MoveRowDown function, as the amount of rows we clear
            // is equal to the amount of rows we must move every row down after the cleared row.

            for(int row = Rows - 1; row >= 0; row--)
            {
                if(IsRowFull(row)) // If we've filled up a row, letting us clear it
                {
                    ClearRow(row); // Clear it
                    clearedRows++; // Increment counter
                }
                else if(clearedRows > 0) // Otherwise, if this row is not to be cleared
                {
                    MoveRowDown(Rows, clearedRows); // Move it downwards
                }
            }
            return clearedRows; // Return the number of rows we cleared (for scorekeeping purposes)
        }
    }

    
}
