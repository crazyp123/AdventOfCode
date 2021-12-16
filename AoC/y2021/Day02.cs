using AoC.Utils;

namespace AoC.y2021;

public class Day02 : Day
{
    private string input = "";
    public Day02()
    {
        input = Input;
    }

    public override object Result1()
    {
        var instr = input.AsListOfPatterns<string, int>("s n");

        var y = 0;
        var x = 0;

        foreach ((string, int) tuple in instr)
        {
            switch (tuple.Item1)
            {
                case "forward":
                    x += tuple.Item2;
                    break;
                case "down":
                    y += tuple.Item2;
                    break;
                case "up":
                    y -= tuple.Item2;
                    break;
            }
        }

        return y * x;
    }

    public override object Result2()
    {
        var instr = input.AsListOfPatterns<string, int>("s n");
        var y = 0;
        var x = 0;
        var z = 0;

        foreach (var (instruction, num) in instr)
        {
            switch (instruction)
            {
                case "forward":
                    x += num;
                    y += (z * num);
                    break;
                case "down":
                    z += num;
                    break;
                case "up":
                    z -= num;
                    break;
            }
        }

        return y * x;
    }
}//1700544575 wrong