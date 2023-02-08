namespace GameOfBattleships.Domain;

public class BattleshipCoordinates
{
    public Coordinates[] Coordinates { get; }

    public BattleshipCoordinates(ICollection<Coordinates> coordinates)
    {
        EnsureCoordinatesAreCorrect(coordinates);
        Coordinates = coordinates.ToArray();
    }

    private void EnsureCoordinatesAreCorrect(ICollection<Coordinates> coordinates)
    {
        // here we could add code that checks if all coordinates are in the same line, consecutive etc.
        // it could be especially useful in the future, but for now I'll let myself skip this part, hope you understand :)
    }

    public bool Contains(Coordinates coordinates)
    {
        return Coordinates.Any(x => x == coordinates);
    }
}