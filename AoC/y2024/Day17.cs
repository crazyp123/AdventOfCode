using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC.Utils;

namespace AoC.y2024;

public class Day17 : Day
{
    private Program _program;

    public Day17()
    {
        // test
        _program = new Program(729, 0, 0, new[] { 0, 1, 5, 4, 3, 0 });
        _program = new Program(56256477, 0, 0, new[] { 2, 4, 1, 1, 7, 5, 1, 5, 0, 3, 4, 3, 5, 5, 3, 0 });
    }

    public override object Result1()
    {
        return _program.Run();
    }

    public override object Result2()
    {
        var res = 0;
        Parallel.For(100000000, 200000000, new ParallelOptions
        {
            MaxDegreeOfParallelism = 32
        }, (l, state) =>
        {
            var p = new Program(l, 0, 0, [2, 4, 1, 1, 7, 5, 1, 5, 0, 3, 4, 3, 5, 5, 3, 0]);
            if (p.Correct())
            {
                res = l;
                state.Stop();
            }
        });
        
        return res;
    }

    private class Program(int a, int b, int c, int[] instructions)
    {
        public int A { get; set; } = a;

        private int B { get; set; } = b;

        private int C { get; set; } = c;

        private int[] Instructions { get; set; } = instructions;

        private List<int> Output { get; set; } = new();

        public string Run()
        {
            var nextInstr = 0;
            while (nextInstr + 1 < Instructions.Length)
            {
                var intr = Instructions[nextInstr];
                var operand = Instructions[nextInstr + 1];
                nextInstr = ExecOp(nextInstr, intr, operand);
            }

            return string.Join(",", Output);
        }

        public bool Correct()
        {
            Run();
            return Output.SequenceEqual(Instructions);
        }

        public int GetComboOperand(int opcode)
        {
            return opcode switch
            {
                0 or 1 or 2 or 3 => opcode,
                4 => A,
                5 => B,
                6 => C,
                _ => throw new ArgumentOutOfRangeException(nameof(opcode))
            };
        }

        public int ExecOp(int pointer, int opcode, int operand)
        {
            try
            {
                switch (opcode)
                {
                    case 0:
                        A = A / (int)Math.Pow(2, GetComboOperand(operand));
                        break;
                    case 1:
                        B = B ^ operand;
                        break;
                    case 2:
                        B = GetComboOperand(operand) % 8;
                        break;
                    case 3:
                        if (A == 0) break;
                        return operand;
                    case 4:
                        B = B ^ C;
                        break;
                    case 5:
                        Output.Add(GetComboOperand(operand) % 8);
                        break;
                    case 6:
                        B = A / (int)Math.Pow(2, GetComboOperand(operand));
                        break;
                    case 7:
                        C = A / (int)Math.Pow(2, GetComboOperand(operand));
                        break;
                }
            }
            catch (ArgumentOutOfRangeException e)
            {
                return 100;
            }

            return pointer + 2;
        }
    }
}