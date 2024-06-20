namespace Tetris.Blocks
{
    public abstract class Block
    {
        // For each block, we're going to contain it within a mini grid of itself. This will allow us to easily spawn and rotate them
        protected abstract Position[][] Tiles { get; } // This will contain the position of the tiles for each of our blocks, as well as their positions for each rotational state
        protected abstract Position StartOffset { get; } // This will decide where exactly the block spawns within our grid
        public abstract int Id { get; } // Id to distinguish between blocks (i.e., which is the line piece, which is the square)

        private int rotationState; // Whether it's at 0 degrees, 90 degrees, 180 degrees or 270 degrees
        private Position offset;

        public Block()
        {
            offset = new Position(StartOffset.Row, StartOffset.Column);
        }

        public IEnumerable<Position> TilePositions()
        {
            foreach (Position p in Tiles[rotationState])
            {
                yield return new Position(p.Row + offset.Row, p.Column + offset.Column);
            }
        }

        public void RotateClockwise() // This lets us rotate our block clockwise
        {
            rotationState = (rotationState + 1) % Tiles.Length; // It will reset to zero once we hit 4, as we only store four states
        }

        public void RotateCounterClockwise() // This lets us rotate our block counter clockwise
        {
            if (rotationState == 0)
            {
                rotationState = Tiles.Length - 1;
            }
            else
            {
                rotationState--;
            }
        }

        public void Move(int rows, int columns) // This will let us move our blocks around
        {
            offset.Row += rows;
            offset.Column += columns;
        }

        public void Reset() // This will let us reset our blocks to their original state
        {
            rotationState = 0;
            offset.Row = StartOffset.Row;
            offset.Column = StartOffset.Column;
        }
    }
}
