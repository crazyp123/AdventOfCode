using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.Objects;
public class SevenSegmentNum
    {
        private readonly Dictionary<string, int> _numberMap = new Dictionary<string, int>
        {
            { "012456", 0 },
            { "25", 1 },
            { "02346", 2 },
            { "02356", 3 },
            { "1235", 4 },
            { "01356", 5 },
            { "013456", 6 },
            { "025", 7 },
            { "0123456", 8 },
            { "012356", 9 },
        };

        private Dictionary<char, int> symbolMap = new Dictionary<char, int>();

        private string _map;

        public string Map
        {
            get => _map;
            set
            {
                _map = value;
                SetSymbolMap(_map);
            }
        }


        public SevenSegmentNum()
        {
            Map = "abcdefg";
        }

        private void SetSymbolMap(string symbols)
        {
            symbolMap.Clear();
            for (var i = 0; i < symbols.Length; i++)
            {
                symbolMap.Add(symbols[i], i);
            }
        }

        public int GetNumFromCode(string code)
        {
            var mapped = code.Select(c => symbolMap[c].ToString());
            var sorted = string.Concat(mapped).Sort();

            return _numberMap.ContainsKey(sorted) ? _numberMap[sorted] : -1;
        }

        public string GetAscii(string code)
        {
            var mapped = code.Select(c => symbolMap[c].ToString());
            return GetBaseAscii(string.Concat(mapped));
        }

        public string GetAsciiNum(int num)
        {
            var code = _numberMap.First(pair => pair.Value == num).Key;
            return GetBaseAscii(code);
        }

        private string GetBaseAscii(string mapped)
        {
            var a = mapped.Contains("0") ? Map[0] : '.';
            var b = mapped.Contains("1") ? Map[1] : '.';
            var c = mapped.Contains("2") ? Map[2] : '.';
            var d = mapped.Contains("3") ? Map[3] : '.';
            var e = mapped.Contains("4") ? Map[4] : '.';
            var f = mapped.Contains("5") ? Map[5] : '.';
            var g = mapped.Contains("6") ? Map[6] : '.';

            var result = $" {a}{a}{a}{a} \n{b}    {c}\n{b}    {c}\n {d}{d}{d}{d} \n{e}    {f}\n{e}    {f}\n {g}{g}{g}{g} \n";
            return result;
        }


    }