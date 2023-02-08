namespace GameOfBattleships.Domain;

public class Battleship
{
    public BattleshipCoordinates Coordinates { get; }
    public Coordinates[] DestroyedCoordinates { get; private set; }
    public bool IsDestroyed => DestroyedCoordinates.Length == Coordinates.Coordinates.Length;

    private Battleship(BattleshipCoordinates coordinates, Coordinates[] destroyedCoordinates)
    {
        Coordinates = coordinates;
        DestroyedCoordinates = destroyedCoordinates;
    }
    
    public static Battleship CreateNew(BattleshipCoordinates coordinates)
    {
        return new Battleship(coordinates, destroyedCoordinates: Array.Empty<Coordinates>());
    }

    public ShipAttackResult ShipAttacked(Coordinates coordinates)
    {
        if (!Coordinates.Contains(coordinates))
            throw new ArgumentException("these coordinates don't belong to this ship");
        if (DestroyedCoordinates.Contains(coordinates))
            return ShipAttackResult.Hit;

        DestroyedCoordinates = DestroyedCoordinates.Append(coordinates).ToArray();

        return IsDestroyed
            ? ShipAttackResult.Sunk
            : ShipAttackResult.Hit;
    }

    public enum ShipAttackResult
    {
        Hit,
        Sunk
    }
}