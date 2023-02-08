using System.Collections.Immutable;

namespace GameOfBattleships.Domain;

internal class BattleshipFactory
{
    private readonly ShipPlacementService shipPlacementService;
    private Random random = new();

    public BattleshipFactory(ShipPlacementService shipPlacementService)
    {
        this.shipPlacementService = shipPlacementService;
    }
    
    public IReadOnlyCollection<Battleship> CreateBattleshipsAtRandomPositions(GameFieldDimensions gameFieldDimensions,
        BattleshipToCreate[] battleshipsToCreate)
    {
        var alreadyCreatedBattleships = new List<Battleship>();

        foreach (var shipToCreate in battleshipsToCreate)
        {
            var newBattleship = CreateBattleship(shipToCreate, gameFieldDimensions, alreadyCreatedBattleships);
            alreadyCreatedBattleships.Add(newBattleship);
        }

        return alreadyCreatedBattleships.ToImmutableArray();
    }

    private Battleship CreateBattleship(BattleshipToCreate shipToCreate, GameFieldDimensions gameFieldDimensions,
        IReadOnlyCollection<Battleship> alreadyCreatedBattleships)
    {
        var shipDirection = random.Next(2) switch
        {
            0 => ShipDirection.Horizontal,
            1 => ShipDirection.Vertical,
            _ => throw new ArgumentException("expected 0 or 1")
        };

        var startField = GetRandomStartField(shipToCreate, shipDirection, gameFieldDimensions,
            alreadyCreatedBattleships);
        var coordinates = GetConsecutiveCoordinates(startField, shipDirection, shipToCreate.Size);

        var shipCoordinates = new BattleshipCoordinates(coordinates);
        return Battleship.CreateNew(shipCoordinates);
    }

    private Coordinates GetRandomStartField(BattleshipToCreate toCreate, ShipDirection shipDirection,
        GameFieldDimensions gameFieldDimensions, IReadOnlyCollection<Battleship> alreadyCreatedBattleships)
    {
        var availableStartFields = shipPlacementService.GetAvailableStartingCoordinates(toCreate, shipDirection,
            gameFieldDimensions, alreadyCreatedBattleships);
        if (availableStartFields.Length == 0)
            throw new Exception("Can't create new battleship - couldn't find suitable start field");
        
        return availableStartFields[random.Next(availableStartFields.Length)];
    }

    private static List<Coordinates> GetConsecutiveCoordinates(Coordinates startField, ShipDirection shipDirection,
        int coordinatesCount)
    {
        var coordinates = new List<Coordinates> {startField};
        for (var i = 1; i < coordinatesCount; i++)
        {
            var nextCoordinate = shipDirection switch
            {
                ShipDirection.Horizontal => coordinates.Last().CoordinateToTheRight(),
                ShipDirection.Vertical => coordinates.Last().CoordinateLower(),
                _ => throw new ArgumentException("unexpected ShipDirection")
            };
            coordinates.Add(nextCoordinate);
        }

        return coordinates;
    }
}