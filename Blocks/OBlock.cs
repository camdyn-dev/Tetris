namespace Tetris.Blocks
{
    public class OBlock : Block
    {
        private readonly Position[][] tiles = new Position[][]
        {
            new Position[] {new(0,0), new(0,1), new(1,0), new(1,1)}
            // We only need one position since the oblock never really rotates
        };

        public override int Id => 4; // Our fourth block

        protected override Position StartOffset => new Position(0, 4);
        protected override Position[][] Tiles => tiles;
    }
}
