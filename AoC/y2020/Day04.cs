using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AoC.Utils;

namespace AoC.y2020
{
    public class Day04 : Day
    {
        private List<Dictionary<string, string>> _passports;

        public Day04()
        {
            _passports = new List<Dictionary<string, string>>();
            var psp = "";
            foreach (var line in (Input+"\n").AsListOf<string>())
            {
                psp += $"{line} ";
                if (string.IsNullOrWhiteSpace(line))
                {
                    var fullPass = psp.Replace("\n", " ").Trim();

                    var passFields = fullPass.Split(" ").ToDictionary(s => s.Substring(0, 3), s => s.Substring(4));
                    _passports.Add(passFields);
                    psp = "";
                }
            }
        }

        public override object Result1()
        {
            var res = 1;

            var mandatory = new string[]
            {
                "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", //"cid"
            };

            res = _passports.Count(pass => mandatory.All(pass.ContainsKey));

            return res;
        }

        public override object Result2()
        {
            var mandatory = new string[]
            {
                "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid", //"cid"
            };

            return _passports.Count(pass =>
            {
                var eycols = new[]
                {
                    "amb","blu","brn","gry","grn","hzl","oth"
                };

                var regex = new Regex("^#([a-f0-9]{6})$");

                return mandatory.All(pass.ContainsKey)
                       && (int.TryParse(pass["byr"], out var byr) && byr >= 1920 && byr <= 2002)
                       && (int.TryParse(pass["iyr"], out var byr1) && byr1 >= 2010 && byr1 <= 2020)
                       && (int.TryParse(pass["eyr"], out var byr2) && byr2 >= 2020 && byr2 <= 2030)
                       && (int.TryParse(pass["hgt"].Replace("in", "").Replace("cm", ""), out var hgt) &&
                           ((pass["hgt"].EndsWith("cm") && hgt >= 150 && hgt <= 193) || (pass["hgt"].EndsWith("in") && hgt >= 59 && hgt <= 76)))
                       && regex.IsMatch(pass["hcl"])
                       && eycols.Contains(pass["ecl"])
                       && (pass["pid"].Length == 9 && pass["pid"].All(char.IsNumber));
            });

        }
    }
}