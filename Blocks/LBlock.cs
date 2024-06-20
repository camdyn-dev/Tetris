namespace Tetris.Blocks
{
    public class LBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] { new Position(0, 2), new Position(1, 0), new Position(1, 1), new Position(1, 2), },
            new Position[] { new Position(0, 1), new Position(1, 1), new Position(2, 1), new Position(2, 2), },
            new Position[] { new Position(1, 0), new Position(1, 1), new Position(1, 2), new Position(2, 0), },
            new Position[] { new Position(0, 0), new Position(0, 1), new Position(1, 1), new Position(2, 1), },

            // This will be all the rotational positions of our piece, i.e.,
            /*
             * O O I | O I O | O O O | - I O
             * - - - | O I O | - - - | O I O
             * O O O | O I - | I O O | O I O
             *
             */
        };

        public override int Id => 3;
        protected override Position StartOffset => new Position(0, 3);
        protected override Position[][] Tiles => tiles;
    }
}
