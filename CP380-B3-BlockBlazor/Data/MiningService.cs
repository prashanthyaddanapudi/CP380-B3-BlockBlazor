using CP380_B1_BlockList.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CP380_B3_BlockBlazor.Data
{
    public class MiningService
    {
        BlockService blockServiceObj = new BlockService();
        PendingTransactionService pendingTransactionServiceObj = new PendingTransactionService();
        public MiningService(BlockService blockService, PendingTransactionService pendingTransactionService)
        {
            blockServiceObj = blockService;
            pendingTransactionServiceObj = pendingTransactionService;
        }
        public async Task<Block> MineBlock(IEnumerable<Block> blockList)
        {
            try
            {
                Block lastBlock = blockList.Last();
                Block block = new Block(DateTime.Now, lastBlock.Hash, pendingTransactionServiceObj.payloads);
                block.Mine(2);

                return block;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
