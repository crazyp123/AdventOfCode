using System.Collections.Generic;
using System.Linq;
using AoC.Utils;

namespace AoC.y2022
{
    public class Day07 : Day
    {
        private Dir _root = new Dir("/", null);

        public Day07()
        {
            var wd = _root;
            foreach (var line in Input.AsListOf<string>())
            {
                var args = line.AsListOf<string>(" ");
                var arg1 = args.First();
                var arg2 = args.Last();

                if (line.StartsWith("$ cd"))
                {
                    wd = arg2 switch
                    {
                        "/" => _root,
                        ".." => wd.Parent,
                        _ => wd.Navigate(arg2)
                    };
                }
                else if (line.StartsWith("dir"))
                {
                    wd.AddDir(arg2);
                }
                else if (char.IsNumber(line[0]))
                {
                    wd.AddFile(arg2, int.Parse(arg1));
                }
            }
        }

        public override object Result1()
        {
            return _root.Flat().Where(d => d.TotalSize() < 100000).Sum(d => d.TotalSize());
        }

        public override object Result2()
        {
            var unusedSpace = 70000000 - _root.TotalSize();

            return _root.Flat()
                .Select(d => d.TotalSize())
                .Where(d => unusedSpace + d >= 30000000)
                .Min();
        }

        class Dir
        {
            public string Name { get; set; }
            public Dictionary<string, Dir> Dirs = new Dictionary<string, Dir>();

            public Dir Parent;

            public List<(string name, int size)> Files = new List<(string name, int size)>();

            public Dir(string name, Dir parent)
            {
                Name = name;
                Parent = parent;
            }

            public Dir Navigate(string path, bool createIfNotExist = true)
            {
                return Dirs.ContainsKey(path) ? Dirs[path] : createIfNotExist ? AddDir(path) : default;
            }

            public Dir AddDir(string path)
            {
                var newDir = new Dir(path, this);
                Dirs.Add(path, newDir);
                return newDir;
            }

            public void AddFile(string name, int size)
            {
                Files.Add((name, size));
            }

            public int TotalSize()
            {
                return Files.Sum(t => t.size) + Dirs.Sum(p => p.Value.TotalSize());
            }

            public IEnumerable<Dir> Flat()
            {
                var list = Dirs.Values.ToList();
                return list.Concat(list.SelectMany(d => d.Flat()));
            }

        }
    }
}