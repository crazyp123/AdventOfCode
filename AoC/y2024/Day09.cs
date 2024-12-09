using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2024;

public class Day09 : Day
{
    private List<int?> _blocks;

    public Day09()
    {
        var test = "2333133121414131402";
        _blocks = GetBlocks(Input);
    }

    public override object Result1()
    {
        var rearranged = ReArrange(_blocks);
        return CheckSum(rearranged);
    }

    public override object Result2()
    {
        var rearranged = ReArrangeFiles(_blocks);
        return CheckSum(rearranged);
    }

    private List<int?> GetBlocks(string input)
    {
        var id = 0;
        var blocks = new List<int?>();

        var isBlock = true;

        for (var i = 0; i < input.Length; i++)
        {
            var c = input[i].AsInt();

            var collection = Enumerable.Range(0, c).Select(_ => isBlock ? id as int? : null).ToList();
            blocks.AddRange(collection);

            if (isBlock) id++;

            isBlock = !isBlock;
        }

        return blocks;
    }

    private int?[] ReArrange(List<int?> blocks)
    {
        var result = new int?[blocks.Count(i => i.HasValue)];

        var reversed = blocks.Select((i, i1) => (num: i, ix: i1)).Where(i => i.num is not null).ToList();
        reversed.Reverse();

        for (var i = 0; i < blocks.Count; i++)
        {
            var current = blocks[i];
            if (current is not null)
            {
                result[i] = current.Value;
                if (i == result.Length - 1) break;
                continue;
            }

            var (num, ix) = reversed.First();
            result[i] = num.Value;
            reversed.RemoveAt(0);
        }

        return result;
    }

    private int?[] ReArrangeFiles(List<int?> blocks)
    {
        var result = blocks.ToArray();

        var blocksList = blocks.FindAdjacentItems().ToList();

        var emptyBlocks = blocksList.Where(t => t.value is null).ToList();
        var blocksToMove = blocksList.Where(t => t.value is not null).OrderByDescending(t => t.value.Value).ToList();

        foreach (var (value, count, ix) in blocksToMove)
        {
            var emptyIx = emptyBlocks.FindIndex(t => t.count >= count && t.index < ix);
            if (emptyIx == -1) continue;
            var empty = emptyBlocks[emptyIx];

            for (var i = 0; i < count; i++)
            {
                result[empty.index + i] = value;
                result[ix + i] = null;
            }

            if (empty.count == count)
                emptyBlocks.RemoveAt(emptyIx);
            else
                emptyBlocks[emptyIx] = (null, empty.count - count, empty.index + count);
        }

        return result.ToArray();
    }

    private ulong CheckSum(int?[] blocks)
    {
        ulong result = 0;
        for (var i = 0; i < blocks.Length; i++)
        {
            var block = blocks[i];
            if (block == null) continue;
            result += (ulong)i * (ulong)block;
        }

        return result;
    }
}