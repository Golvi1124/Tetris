using Tetris.Blocks;

namespace Tetris.Core;

public class BlockQueue
{
    private readonly Block[] blocks = new Block[]
    {
        new IBlock(),
        new JBlock(),
        new LBlock(),
        new OBlock(),
        new SBlock(),
        new TBlock(),
        new ZBlock(),
        new IIBlock(),
        new OOBlock()
    };

    private readonly Random random = new Random();

    public Block NextBlock { get; private set; } // property for the next block in the queue. later will show to player

    public BlockQueue()
    {
        NextBlock = RandomBlock();
    }

    private Block RandomBlock()
    {
        return blocks[random.Next(blocks.Length)];
    }

    public Block GetAndUpdate() // returns next block and updates the property
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