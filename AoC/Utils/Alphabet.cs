namespace AoC.Utils;

public static class Alphabet
{
    public static char[] Characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

    /// <summary>
    /// Returns the position between 1-26 for lowercase and 27-52 for uppercase
    /// </summary>
    public static int GetLetterPosition(char c)
    {
        int index = (int)c % 32;
        if (char.IsUpper(c)) index += 26;
        return index;
    }

    /// <summary>
    /// Returns the position between 1-26 for lowercase and 27-52 for uppercase
    /// </summary>
    public static int PositionInAlphabet(this char c)
    {
        return GetLetterPosition(c);
    }
}