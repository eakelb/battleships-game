namespace GameOfBattleships.Domain;

public class GameField
{
    public Battleship[] Battleships { get; }
    public GameFieldDimensions Dimensions { get; }
    public bool GameWon => Battleships.All(x => x.IsDestroyed);

    private GameField(Battleship[] battleships, GameFieldDimensions dimensions)
    {
        Battleships = battleships;
        Dimensions = dimensions;
    }

    public AttackResult AttackAt(Coordinates coordinates)
    {
        if (GameWon)
            throw new ArgumentException("Game is already won");
        
        if (!IsWithinGameField(coordinates))
            return AttackResult.AttackOutsideOfGameField;
        
        var targetBattleship = Battleships.SingleOrDefault(x => x.Coordinates.Contains(coordinates));
        if (targetBattleship == null)
            return AttackResult.Miss;

        var shipAttackResult = targetBattleship.ShipAttacked(coordinates);
        
        if (shipAttackResult == Battleship.ShipAttackResult.Hit)
            return AttackResult.Hit;
        
        if (GameWon)
            return AttackResult.Win;
        
        return AttackResult.Sunk;
    }
    
    public static GameField CreateNew(GameFieldDimensions dimensions, BattleshipToCreate[] battleshipsToCreate)
    {
        // it's a super small console app I'll let myself skip DI and just use 'new' everywhere :)
        // real-world solutions will of course benefit from DI and nice interface segregation
        // I'll be glad to tell you about what I believe would make for nice interfaces in this project on the review!
        
        var battleshipFactory = new BattleshipFactory(new ShipPlacementService());
        var battleships = battleshipFactory.CreateBattleshipsAtRandomPositions(dimensions, battleshipsToCreate);

        return new GameField(battleships.ToArray(), dimensions);
    }

    private bool IsWithinGameField(Coordinates coordinates)
    {
        return Dimensions.X >= coordinates.GetXAsNumeric() && Dimensions.Y >= coordinates.Y;
    }

    public enum AttackResult
    {
        Miss,
        Hit,
        Sunk,
        Win,
        AttackOutsideOfGameField
    }
}