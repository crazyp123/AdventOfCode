using System;
using System.Collections.Generic;
using AoC.Utils;

namespace AoC.y2019
{
    public class Day5 : Day
    {
        private List<int> _input;
        private int r;

        public Day5()
        {
            _input = Input.AsListOf<int>(',');
            // _input = "3,21,1008,21,8,20,1005,20,22,107,8,21,20,1006,20,31,1106,0,36,98,0,0,1002,21,125,20,4,20,1105,1,46,104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99".AsListOf<int>(',');
        }

        public override object Result1()
        {
            foreach (var i in Run(1))
            {
                Console.WriteLine(i);
            }

            return "done";
        }

        public override object Result2()
        {
            foreach (var i in Run(5))
            {
                Console.WriteLine(i);
            }

            return "done";
        }

        private IEnumerable<int> Run(int input)
        {
            var result = new List<int>();
            var numbers = _input.ToArray();

            for (var i = 0; i < numbers.Length;)
            {
                if (numbers[i] == 99) yield break;

                var instr = numbers[i].ToString();

                instr = instr.Length > 1 ? instr.Remove(instr.Length - 2, 1) : instr;

                var decoded = new int[4];
                for (int j = instr.Length - 1; j >= 0; j--)
                {
                    var n = int.Parse(instr[j].ToString());
                    decoded[instr.Length - 1 - j] = n;
                }

                var x = numbers[i + 1];

                var opcode = decoded[0];
                switch (opcode)
                {
                    case 1: // add
                    case 2:// multiply
                        {
                            var y = numbers[i + 2];

                            var p1 = decoded[1] == 0 ? numbers[x] : x;
                            var p2 = decoded[2] == 0 ? numbers[y] : y;
                            r = decoded[0] == 1 ? p1 + p2 : p1 * p2;

                            numbers[numbers[i + 3]] = r;

                            i += 4;
                            break;
                        }
                    case 3: // input
                        numbers[x] = input;
                        i += 2;
                        break;
                    case 4: // output
                        yield return decoded[1] == 0 ? numbers[x] : x;
                        i += 2;
                        break;
                    case 5: // jump if true
                        {
                            var y = numbers[i + 2];
                            var p1 = decoded[1] == 0 ? numbers[x] : x;
                            var p2 = decoded[2] == 0 ? numbers[y] : y;

                            if (p1 != 0)
                            {
                                i = p2;
                            }
                            else
                            {
                                i += 3;
                            }
                        }
                        break;
                    case 6: //jump if false
                        {
                            var y = numbers[i + 2];
                            var p1 = decoded[1] == 0 ? numbers[x] : x;
                            var p2 = decoded[2] == 0 ? numbers[y] : y;

                            if (p1 == 0)
                            {
                                i = p2;
                            }
                            else
                            {
                                i += 3;
                            }
                        }
                        break;
                    case 7: // less than
                        {
                            var y = numbers[i + 2];
                            var p1 = decoded[1] == 0 ? numbers[x] : x;
                            var p2 = decoded[2] == 0 ? numbers[y] : y;

                            numbers[numbers[i + 3]] = p1 < p2 ? 1 : 0;
                        }
                        i += 4;
                        break;
                    case 8: // equals
                        {
                            var y = numbers[i + 2];
                            var p1 = decoded[1] == 0 ? numbers[x] : x;
                            var p2 = decoded[2] == 0 ? numbers[y] : y;

                            numbers[numbers[i + 3]] = p1 == p2 ? 1 : 0;
                        }
                        i += 4;
                        break;

                }
            }

            // return result;
        }

    }
}