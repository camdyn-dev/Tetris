
using System.Windows.Documents;
using System.Windows.Ink;
using Tetris.Blocks;
using Block = Tetris.Blocks.Block;

namespace Tetris
{
    public class GameState // This will be the class that kinda puts everything together
    {
        private Block _currentBlock;

        public Block CurrentBlock
        {
            get => _currentBlock;
            private set
            {
                _currentBlock = value;
                _currentBlock.Reset(); // This will set the correct start position and rotation
                
                for (int i = 0; i < 2;  i++) // This will make the blocks spawn from the center
                {
                    _currentBlock.Move(1, 0);

                    if(!BlockFits())
                    {
                        CurrentBlock.Move(-1, 0);
                    }
                }
            
            }
        }

        public GameGrid GameGrid { get; }
        public BlockQueue BlockQueue { get; }
        public bool GameOver { get; private set; }
        public int Score { get; private set; }
        public Block HeldBlock { get; private set; }
        public bool CanHold { get; private set; }

        public GameState()
        {
            int defaultRows = 22;
            int defaultColumns = 10; // Default values for a tetris board

            GameGrid = new GameGrid(defaultRows, defaultColumns);
            BlockQueue = new BlockQueue();
            CurrentBlock = BlockQueue.GetAndUpdate(); // Initialize everything
            CanHold = true;
        }

        private bool BlockFits()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                if(!GameGrid.IsCellEmpty(p.Row, p.Column))
                {
                    return false; // Basically, if any of the blocks tiles are outside of the grid, or overlapping with another tile, we return false
                }
            }

            return true; // Otherwise, the block is in a legal position
        }

        public void HoldBlock()
        {
            if (!CanHold)
            {
                return;
            }

            if (HeldBlock == null) // If there's nothing currently held
            {
                HeldBlock = CurrentBlock; // Take the current block and store it
                CurrentBlock = BlockQueue.GetAndUpdate(); // Spawn the next block
            }

            else // If there is
            {
                Block tmp = CurrentBlock; // Temporarily hold this block
                CurrentBlock = HeldBlock; // Bring the held block out of storage
                HeldBlock = tmp; // Store the previous block
            }

            CanHold = false; // This is so we can't spam the hold button. We make it true again after placing a block
        }

        public void RotateBlockClockwise()
        {
            CurrentBlock.RotateClockwise(); // Rotate CW

            if(!BlockFits())
            {
                CurrentBlock.RotateCounterClockwise(); // If it doesn't fit, return to original position
            }
        }

        public void RotateBlockCounterClockwise()
        {
            CurrentBlock.RotateCounterClockwise(); // Rotate CCW

            if (!BlockFits())
            {
                CurrentBlock.RotateClockwise(); // If it doesn't fit, return to original position
            }
        }

        public void MoveBlockLeft()
        {
            CurrentBlock.Move(0, -1); // Move left

            if (!BlockFits())
            {
                CurrentBlock.Move(0, 1); // If it doesn't work, move it back
            }
        }

        public void MoveBlockRight()
        {
            CurrentBlock.Move(0, 1); // Move right

            if (!BlockFits())
            {
                CurrentBlock.Move(0, -1); // If it doesn't work, move it back
            }
        }

        private bool IsGameOver()
        {
            return !(GameGrid.IsRowEmpty(0) && GameGrid.IsRowEmpty(1));
        }

        private void PlaceBlock()
        {
            foreach (Position p in CurrentBlock.TilePositions())
            {
                GameGrid[p.Row, p.Column] = CurrentBlock.Id; // Set all tiles to the ID of the block
            }

            Score += GameGrid.ClearFullRows(); // Clear any rows, and add it to the total score

            if(IsGameOver() )
            {
                GameOver = true; // If game is over, end game
            }
            else
            {
                CurrentBlock = BlockQueue.GetAndUpdate(); // Otherwise, get the next block
                CanHold = true;
            }
        }

        public void MoveBlockDown()
        {
            CurrentBlock.Move(1, 0); // Down one row

            if(!BlockFits())
            {
                CurrentBlock.Move(-1, 0); // Move it back if it doesn't fit
                PlaceBlock(); // Place the block, since it can't go anywhere
            }
        }

        private int TileDropDistance(Position p) // This will let us hard drop a block by showing how many blocks we can hard move down
        {
            int drop = 0;

            while (GameGrid.IsCellEmpty(p.Row + drop + 1, p.Column))
            {
                drop++;
            }
            return drop;
        }

        public int BlockDropDistance()
        {
            int drop = GameGrid.Rows;

            foreach(Position p in CurrentBlock.TilePositions())
            {
                drop = System.Math.Min(drop, TileDropDistance(p));
            }

            return drop;
        }

        public void HardDropBlock()
        {
            CurrentBlock.Move(BlockDropDistance(), 0);
            PlaceBlock();
        }
    }
}
