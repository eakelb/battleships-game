using FluentAssertions;
using GameOfBattleships.Domain;
using NUnit.Framework;

namespace GameOfBattleships.Tests;

public class BattleshipFactoryTests
{
    private static BattleshipFactory CreateSut()
    {
        return new BattleshipFactory(new ShipPlacementService());
    }
    
    [Test]
    [Repeat(100)]
    public void CreateBattleshipsAtRandomPositions_ShouldNeverCreateOverlappingShips()
    {
        var factory = CreateSut();
        var dimensions = new GameFieldDimensions(5, 5);
        var battleshipsToCreate = new[]
        {
            new BattleshipToCreate(4),
            new BattleshipToCreate(3),
            new BattleshipToCreate(2),
        };

        var result = factory.CreateBattleshipsAtRandomPositions(dimensions, battleshipsToCreate);

        var allCoordinates = result.SelectMany(x => x.Coordinates.Coordinates).ToArray();
        var allCoordinatesDeduplicated = allCoordinates.ToHashSet().ToArray();

        allCoordinates.Should().BeEquivalentTo(allCoordinatesDeduplicated);
    }
    
    [Test]
    public void CreateBattleshipsAtRandomPositions_ShouldThrow_WhenItsImpossibleToLocateShips()
    {
        var factory = CreateSut();
        var dimensions = new GameFieldDimensions(5, 5);
        var battleshipsToCreate = new[]
        {
            new BattleshipToCreate(Size: 5),
            new BattleshipToCreate(Size: 5),
            new BattleshipToCreate(Size: 5),
            new BattleshipToCreate(Size: 5),
            new BattleshipToCreate(Size: 5),
            new BattleshipToCreate(Size: 1),
        };

        var act = () => factory.CreateBattleshipsAtRandomPositions(dimensions, battleshipsToCreate);

        act.Should().Throw<Exception>();
    }
}