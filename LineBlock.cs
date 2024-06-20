namespace Tetris
{
    public class LineBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new Position(1, 0), new Position(1, 1), new Position(1, 2), new Position(1, 3), },
            new Position[] { new Position(0, 2), new Position(2, 2), new Position(2, 2), new Position(3, 2), },
            new Position[] { new Position(2, 0), new Position(2, 1), new Position(2, 2), new Position(2, 3), },
            new Position[] { new Position(0, 1), new Position(1, 1), new Position(2, 1), new Position(3, 1), },

            // This will be all the rotational positions of our line piece, i.e.,
            /*
             * O O O O | O O I O | O O O O | O I O O
             * - - - - | O O I O | O O O O | O I O O
             * O O O O | O O I O | - - - - | O I O O
             * O O O O | O O I O | O O O O | O I O O
             * 
             */
        };
    }
}
