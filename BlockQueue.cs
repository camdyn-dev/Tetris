using Tetris.Blocks;

namespace Tetris
{
    public class BlockQueue // This will let us put new blocks into the queue, dropping them into the grid
    {
        private readonly Block[] blocks = new Block[]
        {
            new IBlock(),
            new JBlock(),
            new LBlock(),
            new OBlock(),
            new SBlock(),
            new TBlock(),
            new ZBlock()
        };

        private readonly Random random = new Random(); // Random generator to randomly pick a number for the block

        public Block NextBlock { get; private set; } // This will show what the next block is, so we know what to expect

        public BlockQueue()
        {
            NextBlock = RandomBlock();
        }

        private Block RandomBlock() // This will return a random block
        {
            return blocks[random.Next(blocks.Length)];
        }

        public Block GetAndUpdate() // This is to make sure we don't get the same block two times in a row
        {
            Block block = NextBlock;

            do
            {
                NextBlock = RandomBlock();
            }
            while (block.Id == NextBlock.Id);

            return block;
        }

    }

    
}
