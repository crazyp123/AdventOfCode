using System;
using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2021
{
    public class Day16 : Day
    {
        private string TestInput;
        private Packet _packet;

        class Packet
        {
            public int Version;
            public int TypeId;

            public long? Literal;

            public List<Packet> SubPackets;

            public Packet(int version, int typeId)
            {
                Version = version;
                TypeId = typeId;
                SubPackets = new List<Packet>();
            }

            public List<Packet> Flatten()
            {
                return new List<Packet> { this }.Concat(SubPackets.SelectMany(x => x.Flatten())).ToList();
            }

            public long Compute()
            {
                switch (TypeId)
                {
                    case 0:
                        return SubPackets.Sum(p => p.Compute());
                    case 1:
                        return SubPackets.Aggregate(1L, (res, p) => res * p.Compute());
                    case 2:
                        return SubPackets.Min(p => p.Compute());
                    case 3:
                        return SubPackets.Max(p => p.Compute());
                    case 4:
                        return Literal.Value;
                    case 5:
                        return SubPackets[0].Compute() > SubPackets[1].Compute() ? 1 : 0;
                    case 6:
                        return SubPackets[0].Compute() < SubPackets[1].Compute() ? 1 : 0;
                    case 7:
                        return SubPackets[0].Compute() == SubPackets[1].Compute() ? 1 : 0;
                }
                return 0;
            }
        }

        public Day16()
        {
            // test input
            TestInput = "9C0141080250320F1802104A08";

            _packet = Decode(Input);
        }

        Packet Decode(string input)
        {
            var parts = input.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2)).ToList();
            var binary = string.Concat(parts.Select(s => s.PadLeft(4, '0')));
            
            return ParsePacket(binary).Item1;
        }

        (Packet, string) ParsePacket(string binary)
        {
            var version = Convert.ToInt32(binary.Substring(0, 3), 2);
            var id = Convert.ToInt32(binary.Substring(3, 3), 2);

            binary = binary.Remove(0, 6);
            var packet = new Packet(version, id);

            if (id == 4)
            {
                var (n, x) = GetNextLiteral(binary);
                packet.Literal = n;
                binary = x;
            }
            else
            {
                var i = binary[0];
                binary = binary.Remove(0, 1);

                if (i == '0') //total length
                {
                    var length = Convert.ToInt32(binary.Substring(0, 15), 2);
                    binary = binary.Remove(0, 15);

                    var (x, y) = GetSubpackets(binary.Substring(0, length));
                    packet.SubPackets = x;
                    binary = binary.Remove(0, length);
                }
                else // count subpackets
                {
                    var count = Convert.ToInt32(binary.Substring(0, 11), 2);
                    binary = binary.Remove(0, 11);

                    var (x, y) = GetSubpackets(binary, count);
                    packet.SubPackets = x;
                    binary = y;
                }
            }

            return (packet, binary);
        }


        (List<Packet>, string) GetSubpackets(string value, int count = 0)
        {
            var res = new List<Packet>();

            while (value.TrimEnd('0').Length > 0 && (count == 0 || res.Count < count))
            {
                var (x, y) = ParsePacket(value);
                res.Add(x);
                value = y;
            }
            return (res, value);
        }

        static (long, string) GetNextLiteral(string binary)
        {
            var num = "";
            var go = '1';

            while (go == '1')
            {
                var bits = binary.Substring(0, 5);

                go = bits[0];
                num += bits.Substring(1);

                binary = binary.Remove(0, 5);
            }
            return (Convert.ToInt64(num, 2), binary);
        }

        public override object Result1()
        {
            
            var flatten = _packet.Flatten().Select(p => p.Version).ToList();
            return flatten.Sum();
        }

        public override object Result2()
        {
            return _packet.Compute();
        }
    }
}