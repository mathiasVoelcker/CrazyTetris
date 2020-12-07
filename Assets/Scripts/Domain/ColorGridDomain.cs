using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MathTetris.Assets.Scripts.Domain
{
    public class ColorGridDomain
    {
        
        public int _lines = 12;
        public int _columns = 10;
        private Transform[,] _grid;

        public ColorGridDomain(Transform[,] grid, int lines, int columns)
        {
            _lines = lines;
            _columns = columns;
            _grid = grid;
        }

        public List<Block> CheckColors(int groupSize)
        {
            var blocksToRemove = new List<Block>();

            var blocksChecked = new bool [_lines, _columns];
            for (int l = 0; l < _lines; l++)
            {
                for (int c = 0; c < _columns; c++)
                {
                    var blocksToCheck = new List<Block>();
                    var blockTransform = _grid[l, c];
                    if (blockTransform == null) continue;
                    var block = new Block(blockTransform, l, c);
                    if (!blocksChecked[l, c] && !block.IsSpecialBlock())
                    {
                        var blocksInGroup = GroupBlocks(block, blocksToCheck, blocksChecked, groupSize);
                        if (blocksInGroup >= groupSize)
                        {
                            foreach (var specialBlock in blocksToCheck.Where(x => x.IsSpecialBlock()).ToList())
                                specialBlock.BlockObject.SetColor(block.BlockObject.GetColor());
                            blocksToRemove.AddRange(blocksToCheck);
                        }
                        foreach (var specialBlock in blocksToCheck.Where(x => x.IsSpecialBlock()).ToList())
                            blocksChecked[specialBlock.Line, specialBlock.Column] = false;
                    }
                }
            }
            return blocksToRemove;
        }
        
        private int GroupBlocks(Block block, List<Block> blocksToCheck, bool [,] blocksChecked, int groupSize)
        {
            var blocksInGroup = 1;
            blocksToCheck.Add(block);
            blocksChecked[block.Line, block.Column] = true;
            Queue<Block> blocksQueue = new Queue<Block>();
            blocksQueue.Enqueue(block);
            Color colorFocus = block.BlockObject.GetColor();

            while (blocksQueue.Count != 0 )
            {
                block = blocksQueue.Dequeue();
                var adjacentBlocks = GetAdjacentBlocks(block);
                foreach (var adjBlock in adjacentBlocks)
                {
                    //if (adjBlock.IsSpecialBlock())
                    //{
                    //    adjBlock.IsSpecial = true;
                    //}
                    if ((adjBlock.IsSpecialBlock() ||
                        adjBlock.BlockObject.GetColor() == colorFocus) && !blocksChecked[adjBlock.Line, adjBlock.Column])
                    {
                        blocksQueue.Enqueue(adjBlock);
                        blocksToCheck.Add(adjBlock);
                        blocksInGroup++;
                        //if (!adjBlock.IsSpecialBlock())
                        blocksChecked[adjBlock.Line, adjBlock.Column] = true;
                        if (blocksInGroup == groupSize) return blocksInGroup;
                    }

                }
            }
           
            return blocksInGroup;
        }

        private List<Block> GetAdjacentBlocks(Block block)
        {
            int line, column;
            var adjacentBlocks = new List<Block>();
            if (block.Line < _lines - 1)
            {
                line = block.Line + 1;
                column = block.Column;
                if (_grid[line, column] != null)
                    adjacentBlocks.Add(new Block(_grid[line, column], line, column));
            }
            if (block.Line > 0)
            {
                line = block.Line - 1;
                column = block.Column;
                if (_grid[line, column] != null)
                    adjacentBlocks.Add(new Block(_grid[line, column], line, column));
            }
            if (block.Column < _columns - 1)
            {
                line = block.Line;
                column = block.Column + 1;
                if (_grid[line, column] != null)
                    adjacentBlocks.Add(new Block(_grid[line, column], line, column));
            }
            if (block.Column > 0)
            {
                line = block.Line;
                column = block.Column -1;
                if (_grid[line, column] != null)
                    adjacentBlocks.Add(new Block(_grid[line, column], line, column));
            }
            return adjacentBlocks;
        }
    }
}