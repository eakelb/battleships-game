using System.Text.RegularExpressions;

namespace GameOfBattleships.Domain;

public record Coordinates
{
    public char X { get; }
    public int Y { get; }
    
    public Coordinates(char x, int y)
    {
        var xRegex = new Regex("[A-Z]");
        if (!xRegex.IsMatch(x.ToString()))
            throw new Exception("X should be an uppercase letter from A to Z");
        if (y < 1)
            throw new Exception("Y cannot be lower than 1");
        
        X = x;
        Y = y;
    }

    /// <summary>
    /// X represented as numeric value, starting from 1 
    /// </summary>
    public static Coordinates FromNumeric(int x, int y)
    {
        if (x < 1 || y < 1) throw new Exception("coordinates have to start from 1");
        var xAsChar = (char) ('A' + (x - 1));
        return new Coordinates(xAsChar, y);
    }

    /// <summary>
    ///  X as corresponding numeric value, starting from 1
    /// </summary>
    public int GetXAsNumeric() => (X - 'A') + 1;

    public Coordinates CoordinateToTheRight()
    {
        if (X == 'Z')
            throw new Exception("Cannot go further to the right than 'Z' - not supported yet");
        
        return new((char)(X + 1), Y);
    }
    
    public Coordinates CoordinateLower()
    {
        return new(X, Y + 1);
    }
}
