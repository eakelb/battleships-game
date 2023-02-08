using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using GameOfBattleships.Domain;

namespace GameOfBattleships;

internal class CoordinatesFactory
{
    public static bool TryCreateFromUserInput(string userInput, [NotNullWhen(true)]out Coordinates? coordinates)
    {
        coordinates = null;

        try
        {
            var sanitizedInput = userInput.Trim().ToUpperInvariant();

            var correctInputRegex = new Regex("^[A-Z][0-9]{1,2}$");
            if (!correctInputRegex.IsMatch(sanitizedInput)) return false;

            var x = sanitizedInput[0];
            var y = int.Parse(sanitizedInput[1..]);

            coordinates = new Coordinates(x, y);
            return true;
        }
        catch
        {
            return false;
        }
    }
}